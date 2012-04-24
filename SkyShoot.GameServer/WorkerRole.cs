using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using SkyShoot.Contracts.Service;
using SkyShoot.GameServer.Logger;

namespace SkyShoot.GameServer
{
    public class WorkerRole : RoleEntryPoint
    {
        private ServiceHost _host;

        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            Trace.WriteLine("$projectname$ entry point called", "Information");

            while (true)
            {
                Thread.Sleep(10000);
                Trace.WriteLine("Working", "Information");
            }
        }

        public override void OnStop()
        {
            _host.Close();
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

                
                _host = new ServiceHost(typeof(MainSkyShootService));

                _host.AddServiceEndpoint(
                    typeof (ISkyShootService),
                    new NetTcpBinding(SecurityMode.None)
                    {
                        CloseTimeout = new TimeSpan(1, 0, 0, 0),
                        OpenTimeout = new TimeSpan(1, 0, 0, 0),
                        ReceiveTimeout = new TimeSpan(1, 0, 0, 0),
                        SendTimeout = new TimeSpan(1, 0, 0, 0)
                    },
                    string.Format("net.tcp://{0}", externalEndPoint.IPEndpoint));

                var metadataBehavior =
                        _host.Description.Behaviors.Find<ServiceMetadataBehavior>();
                if (metadataBehavior == null)
                {
                    metadataBehavior = new ServiceMetadataBehavior { HttpGetEnabled = false };
                    _host.Description.Behaviors.Add(metadataBehavior);
                }

                /*host.AddServiceEndpoint(typeof(IMetadataExchange),
                        MetadataExchangeBindings.CreateMexTcpBinding(),
                        "mex");*/

                _host.Open();

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
