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

			host.AddServiceEndpoint(typeof(ISkyShootGameService),
															new NetTcpBinding(SecurityMode.None), "SkyShootService");

			var metadataBehavior =
					host.Description.Behaviors.Find<ServiceMetadataBehavior>(); 
			if (metadataBehavior == null)
			{
				metadataBehavior = new ServiceMetadataBehavior {HttpGetEnabled = false};
				host.Description.Behaviors.Add(metadataBehavior);
			}

			host.AddServiceEndpoint(typeof(IMetadataExchange),
					MetadataExchangeBindings.CreateMexTcpBinding(),
					"mex");

			host.Open();
			Console.WriteLine("Started!");
			Console.ReadKey();
			host.Close();

		}
	}
}
