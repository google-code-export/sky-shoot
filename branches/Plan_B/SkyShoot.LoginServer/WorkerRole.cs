using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Routing;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using SkyShoot.Contracts.Service;

namespace SkyShoot.LoginServer
{
    public class WorkerRole : RoleEntryPoint
    {
        private ServiceHost _routingHost, _serviceHost;

        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            Trace.WriteLine("$projectname$ entry point called", "Information");

            while (true)
            {
                Thread.Sleep(10000);
                //Trace.WriteLine("Working", "Information");
            }
        }

        public override void OnStop()
        {
            _routingHost.Close();
            _serviceHost.Close();
            base.OnStop();
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 100500;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.


            try
            {

                //Trace.Listeners.Add(new TableTraceListener());
                //Trace.WriteLine(args);
                RoleInstanceEndpoint externalEndPoint =
                    RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["ServiceEndpoint"];

                _routingHost = new ServiceHost(typeof(RoutingService));
                var table = new MessageFilterTable<IEnumerable<ServiceEndpoint>>();

                _routingHost.AddServiceEndpoint(typeof (IRequestReplyRouter),
                                                new NetTcpBinding(SecurityMode.None)
                                                    {
                                                        CloseTimeout = new TimeSpan(1, 0, 0, 0),
                                                        OpenTimeout = new TimeSpan(1, 0, 0, 0),
                                                        ReceiveTimeout = new TimeSpan(1, 0, 0, 0),
                                                        SendTimeout = new TimeSpan(1, 0, 0, 0)
                                                    },
                                                string.Format("net.tcp://{0}", externalEndPoint.IPEndpoint));

                _serviceHost = new ServiceHost(new SkyShootLoginService(table));
                externalEndPoint =
                    RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["LoginEndpoint"];
                _serviceHost.AddServiceEndpoint(
                    typeof(ISkyShootLogin),
                    new NetTcpBinding(SecurityMode.None)
                    {
                        CloseTimeout = new TimeSpan(1, 0, 0, 0),
                        OpenTimeout = new TimeSpan(1, 0, 0, 0),
                        ReceiveTimeout = new TimeSpan(1, 0, 0, 0),
                        SendTimeout = new TimeSpan(1, 0, 0, 0)
                    },
                    string.Format("net.tcp://{0}", externalEndPoint.IPEndpoint));
                table.Add(
                    new KeyValuePair<MessageFilter, IEnumerable<ServiceEndpoint>>(
                        new SkyShootMessageFilter(Guid.NewGuid().ToString(), true),
                        new List<ServiceEndpoint>()
                            {
                                new ServiceEndpoint(
                                    ContractDescription.GetContract(
                                        typeof (ISkyShootLogin)),
                                    new NetTcpBinding(SecurityMode.None)
                                        {
                                            CloseTimeout = new TimeSpan(1, 0, 0, 0),
                                            OpenTimeout = new TimeSpan(1, 0, 0, 0),
                                            ReceiveTimeout = new TimeSpan(1, 0, 0, 0),
                                            SendTimeout = new TimeSpan(1, 0, 0, 0)
                                        },
                                    new EndpointAddress(
                                        string.Format("net.tcp://{0}", externalEndPoint.IPEndpoint)))
                            }));
                var routingBehavior =
                    new RoutingBehavior(new RoutingConfiguration(table, true));
                _routingHost.Description.Behaviors.Add(routingBehavior);
                var serviceBehavior = _serviceHost.Description.Behaviors.Find<ServiceBehaviorAttribute>();
                if(serviceBehavior==null)
                {
                    serviceBehavior = new ServiceBehaviorAttribute();
                    _serviceHost.Description.Behaviors.Add(serviceBehavior);
                }
                serviceBehavior.IncludeExceptionDetailInFaults = true;
                serviceBehavior = _routingHost.Description.Behaviors.Find<ServiceBehaviorAttribute>();
                if (serviceBehavior == null)
                {
                    serviceBehavior = new ServiceBehaviorAttribute();
                    _serviceHost.Description.Behaviors.Add(serviceBehavior);
                }
                serviceBehavior.IncludeExceptionDetailInFaults = true;
                var metadataBehavior =
                        _serviceHost.Description.Behaviors.Find<ServiceMetadataBehavior>();
                if (metadataBehavior == null)
                {
                    metadataBehavior = new ServiceMetadataBehavior { HttpGetEnabled = false };
                    _serviceHost.Description.Behaviors.Add(metadataBehavior);
                }

                /*host.AddServiceEndpoint(typeof(IMetadataExchange),
                        MetadataExchangeBindings.CreateMexTcpBinding(),
                        "mex");*/

                _routingHost.Open();
                _serviceHost.Open();

            }
            catch (Exception exc)
            {
                //Trace.WriteLine("Server crashed: " + exc);
                throw;
            }
            return base.OnStart();
        }
    }
}
