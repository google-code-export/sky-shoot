using System;

using System.Net;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Description;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace SkyShoot.Service
{
    public class WorkerRole : RoleEntryPoint
    {

        private const int ConnectionLimit = 10;  

        private ServiceHost _serviceHost;

        public override void Run()
        {
            StartWcfService();
            while (true)
            {
                
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = ConnectionLimit;

            return base.OnStart();
        }

        private void StartWcfService()
        {
            RoleInstanceEndpoint externalEndPoint =
                RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["SkyShootEndpoint"];
            _serviceHost = new ServiceHost(typeof(MainSkyShootService),
                                  new Uri(String.Format("net.tcp://{0}/", externalEndPoint.IPEndpoint)));
            
            var metadataBehavior =
                _serviceHost.Description.Behaviors.Find<ServiceMetadataBehavior>();
            if (metadataBehavior == null)
            {
                metadataBehavior = new ServiceMetadataBehavior();
                _serviceHost.Description.Behaviors.Add(metadataBehavior);
            }

            _serviceHost.AddServiceEndpoint(typeof(Contracts.Service.ISkyShootService), new NetTcpBinding(),
                                   "SkyShootService");
            _serviceHost.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexTcpBinding(),
                                   "mex");

            try
            {
                _serviceHost.Open();
                Trace.TraceInformation("WCF service host started successfully.");
            }
            catch (TimeoutException timeoutException)
            {
                Trace.TraceError("The service operation timed out. {0}",
                                 timeoutException.Message);
            }
            catch (CommunicationException communicationException)
            {
                Trace.TraceError("Could not start WCF service host. {0}",
                                 communicationException.Message);
            }

        }
    }
}
