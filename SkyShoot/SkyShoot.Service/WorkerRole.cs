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

		private ServiceHost _serviceHost;
		private ServiceHost _gameHost;

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
			RoleInstanceEndpoint firstExternalEndPoint =
				RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["SkyShootEndpoint"];
			_serviceHost = new ServiceHost(typeof(MainSkyShootService),
								  new Uri(String.Format("net.tcp://{0}/", firstExternalEndPoint.IPEndpoint)));
			
			var firstMetadataBehavior =
				_serviceHost.Description.Behaviors.Find<ServiceMetadataBehavior>();
			if (firstMetadataBehavior == null)
			{
				firstMetadataBehavior = new ServiceMetadataBehavior();
				_serviceHost.Description.Behaviors.Add(firstMetadataBehavior);
			}

			_serviceHost.AddServiceEndpoint(typeof(Contracts.Service.ISkyShootAdministratorService), new NetTcpBinding(SecurityMode.None),
								   "SkyShootAdministratorService");

			try
			{
				_serviceHost.Open();
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

			RoleInstanceEndpoint secondExternalEndPoint =
				RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["SkyShootEndpoint"];
			_gameHost = new ServiceHost(typeof(MainSkyShootService),
								  new Uri(String.Format("net.tcp://{0}/", secondExternalEndPoint.IPEndpoint)));
			
			var secondMetadataBehavior =
				_serviceHost.Description.Behaviors.Find<ServiceMetadataBehavior>();
			if (secondMetadataBehavior == null)
			{
				secondMetadataBehavior = new ServiceMetadataBehavior();
				_serviceHost.Description.Behaviors.Add(firstMetadataBehavior);
			}

			_gameHost.AddServiceEndpoint(typeof(Contracts.Service.ISkyShootGameService), new NetTcpBinding(SecurityMode.None),
								   "SkyShootGameService");

			try
			{
				_gameHost.Open();
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
