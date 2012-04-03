using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Diagnostics;
using SkyShoot.Service.Logger;
using SkyShoot.Service;
using SkyShoot.Contracts.Service;
using System.ServiceModel.Routing;
using System.ServiceModel.Dispatcher;
using System.Collections.Generic;
using SkyShoot.Service.Session;
using SkyShoot.ServProgram;


namespace SkyShoot.ServProgram
{
	

	class Program
	{
		public static void host_Faulted(object sender, EventArgs e)
		{
		}

		static void Main(string[] args)
		{
			Trace.Listeners.Add(new TableTraceListener());
			Trace.WriteLine(args);

			var table = new MessageFilterTable<IEnumerable<ServiceEndpoint>>();
			table.Add(new KeyValuePair<MessageFilter, IEnumerable<ServiceEndpoint>>(new SkyShootMessageFilter(SessionManager.Instances[0].ManagerId, false),
																					new List<ServiceEndpoint>()
                                                                                {
                                                                                  new ServiceEndpoint(
                                                                                    ContractDescription.GetContract(
                                                                                      typeof (ISkyShootService)),
                                                                                    new NetTcpBinding(), 
                                                                                    new EndpointAddress(
                                                                                      "net.tcp://localhost:778"))
                                                                                }));

			table.Add(new KeyValuePair<MessageFilter, IEnumerable<ServiceEndpoint>>(new SkyShootMessageFilter(SessionManager.Instances[1].ManagerId, false),
																					new List<ServiceEndpoint>()
                                                                                {
                                                                                  new ServiceEndpoint(
                                                                                    ContractDescription.GetContract(
                                                                                      typeof (ISkyShootService)),
                                                                                    new NetTcpBinding(), 
                                                                                    new EndpointAddress(
                                                                                      "net.tcp://localhost:779"))
                                                                                }));
			table.Add(new KeyValuePair<MessageFilter, IEnumerable<ServiceEndpoint>>(new SkyShootMessageFilter(Guid.NewGuid(), true),
																				  new List<ServiceEndpoint>()
                                                                                {
                                                                                  new ServiceEndpoint(
                                                                                    ContractDescription.GetContract(
                                                                                      typeof (ISkyShootLogin)),
                                                                                    new NetTcpBinding(), 
                                                                                    new EndpointAddress(
                                                                                      "net.tcp://localhost:780"))
                                                                                }));
			var routingBehavior =
			  new RoutingBehavior(new RoutingConfiguration(table, true));


			var host = new ServiceHost(typeof(RoutingService));

			host.AddServiceEndpoint(typeof(IRequestReplyRouter),
															new NetTcpBinding(), "net.tcp://localhost:777");
			host.Description.Behaviors.Add(routingBehavior);
			//host.Faulted += new System.EventHandler(host_Faulted);
			//(host.Description.Behaviors.First(x => x.GetType() == typeof(ServiceDebugBehavior)) as
			//ServiceDebugBehavior).IncludeExceptionDetailInFaults = true;

			/*
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

			*/
			var host1 = new ServiceHost(typeof(MainSkyShootService));
			host1.AddServiceEndpoint(typeof(ISkyShootService), new NetTcpBinding(), "net.tcp://localhost:778");
			host1.Open();

			var host2 = new ServiceHost(typeof(MainSkyShootService));
			host2.AddServiceEndpoint(typeof(ISkyShootService), new NetTcpBinding(), "net.tcp://localhost:779");
			host2.Open();

			var loginHost = new ServiceHost(typeof(SkyShootLoginService));
			loginHost.AddServiceEndpoint(typeof(ISkyShootLogin), new NetTcpBinding(), "net.tcp://localhost:780");
			loginHost.Open();

			host.Open();
			Console.WriteLine("Started!");
			Console.ReadKey();
			host.Close();
		}

		
	}
}
