using System;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Description;
using SkyShoot.Contracts.Service;
using SkyShoot.Contracts.Utils;
using SkyShoot.Service;

namespace SkyShoot.ServProgram
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
#if DEBUG
				Logger.ServerLogger = new Logger(Logger.SolutionPath + "\\logs\\server_log.txt");
#else
				Logger.ServerLogger = new Logger("@server_log.txt");
#endif
				Trace.Listeners.Add(Logger.ServerLogger);

				Trace.WriteLine(args);

				var host = new ServiceHost(typeof(MainSkyShootService), new Uri("net.tcp://localhost:555"));

				host.AddServiceEndpoint(typeof(ISkyShootService), new NetTcpBinding(SecurityMode.None), "SkyShootService");

				var metadataBehavior =
						host.Description.Behaviors.Find<ServiceMetadataBehavior>();
				if (metadataBehavior == null)
				{
					metadataBehavior = new ServiceMetadataBehavior { HttpGetEnabled = false };
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
			catch (Exception exc)
			{
				Trace.WriteLine("Server crashed: " + exc);
				throw;
			}
		}
	}
}
