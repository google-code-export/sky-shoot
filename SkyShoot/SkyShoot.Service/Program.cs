using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Diagnostics;
using SkyShoot.Service.Logger;
using SkyShoot.Service;
using SkyShoot.Contracts.Service;

namespace SkyShoot.ServProgram
{
	class Program
	{
		static void Main(string[] args)
		{
			Trace.Listeners.Add(new TableTraceListener());
			Trace.WriteLine(args);

			var host = new ServiceHost(typeof(MainSkyShootService),
				new Uri("net.tcp://localhost:777"));

			host.AddServiceEndpoint(typeof(ISkyShootAdministratorService),
															new NetTcpBinding(SecurityMode.None), "SkyShootAdministratorService");

			var firstMetadataBehavior =
					host.Description.Behaviors.Find<ServiceMetadataBehavior>(); 
			if (firstMetadataBehavior == null)
			{
				firstMetadataBehavior = new ServiceMetadataBehavior {HttpGetEnabled = false};
				host.Description.Behaviors.Add(firstMetadataBehavior);
			}

			host.AddServiceEndpoint(typeof(IMetadataExchange),
					MetadataExchangeBindings.CreateMexTcpBinding(),
					"mex");

			host.Open();
			Console.WriteLine("Started!");
			Console.ReadKey();
			host.Close();

			var game = new ServiceHost(typeof(MainSkyShootService),
				new Uri("net.tcp://localhost:777"));

			host.AddServiceEndpoint(typeof(ISkyShootGameService),
															new NetTcpBinding(SecurityMode.None), "SkyShootGameService");

			var secondMetadataBehavior =
					host.Description.Behaviors.Find<ServiceMetadataBehavior>();
			if (secondMetadataBehavior == null)
			{
				secondMetadataBehavior = new ServiceMetadataBehavior { HttpGetEnabled = false };
				host.Description.Behaviors.Add(firstMetadataBehavior);
			}

			host.AddServiceEndpoint(typeof(IMetadataExchange),
					MetadataExchangeBindings.CreateMexTcpBinding(),
					"mex");

			game.Open();
			Console.WriteLine("Started!");
			Console.ReadKey();
			game.Close();
		}
	}
}
