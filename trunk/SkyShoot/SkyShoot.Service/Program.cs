using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace SkyShoot.ServProgram
{
	class Program
	{
		static void Main(string[] args)
		{
			var host = new ServiceHost(typeof(SkyShoot.Service.MainSkyShootService),
		new Uri("net.tcp://localhost:8002"));

			host.AddServiceEndpoint(typeof(SkyShoot.Contracts.Service.ISkyShootService),
															new NetTcpBinding(SecurityMode.None), "SkyShootService");

			var metadataBehavior =
					host.Description.Behaviors.Find<ServiceMetadataBehavior>();
			if (metadataBehavior == null)
			{
				metadataBehavior = new ServiceMetadataBehavior();
				metadataBehavior.HttpGetEnabled = false;
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
