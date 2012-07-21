using System;

using System.Net;

using System.Threading;
using System.Diagnostics;
using System.ServiceModel;

using System.ServiceModel.Description;

using Microsoft.WindowsAzure.ServiceRuntime;

namespace SkyShoot.Service
{
	public class WorkerRole : RoleEntryPoint
	{

		private const int ConnectionLimit = 10;  

		private ServiceHost _host;

		public override void Run()
		{
			Trace.WriteLine("SkyShootWorkerRole entry point called");

			StartWcfService();

			Trace.WriteLine("SkyShootService is running!");

			while (true)
			{
				Thread.Sleep(10000);
				Trace.WriteLine("Working...");
			}
		}

		public override bool OnStart()
		{
			Trace.Listeners.Add(new SkyShoot.Service.Logger.TableTraceListener());
			
			// Set the maximum number of concurrent connections 
			ServicePointManager.DefaultConnectionLimit = ConnectionLimit;

			return base.OnStart();
		}

		private void StartWcfService()
		{
			RoleInstanceEndpoint externalEndPoint =
				RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["SkyShootEndpoint"];
			_host = new ServiceHost(typeof(MainSkyShootService),
								  new Uri(String.Format("net.tcp://{0}/", externalEndPoint.IPEndpoint)));
			
			var metadataBehavior =
				_host.Description.Behaviors.Find<ServiceMetadataBehavior>();
			if (metadataBehavior == null)
			{
				metadataBehavior = new ServiceMetadataBehavior();
				_host.Description.Behaviors.Add(metadataBehavior);
			}

			_host.AddServiceEndpoint(typeof(Contracts.Service.ISkyShootService), new NetTcpBinding(SecurityMode.None),
								   "SkyShootService");

			try
			{
				_host.Open();
				Trace.WriteLine("WCF service host started successfully.");
			}
			catch (TimeoutException timeoutException)
			{
				Trace.Fail("The service operation timed out. {0} " +
								 timeoutException.Message);
			}
			catch (CommunicationException communicationException)
			{
				Trace.Fail("Could not start WCF service host. {0} " +
								 communicationException.Message);
			}
		}
	}
}
