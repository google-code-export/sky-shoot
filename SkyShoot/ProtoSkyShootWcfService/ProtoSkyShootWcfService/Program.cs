using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;

namespace ProtoSkyShootWcfService
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new ServiceHost(typeof (MyService), 
                new Uri("net.tcp://localhost:8002"));

            host.AddServiceEndpoint(typeof (IMyService),
                                    new NetTcpBinding(SecurityMode.None), "");

            var metadataBehavior =
                host.Description.Behaviors.Find<ServiceMetadataBehavior>();
            if (metadataBehavior == null)
            {
                metadataBehavior = new ServiceMetadataBehavior();
                metadataBehavior.HttpGetEnabled = false;
                host.Description.Behaviors.Add(metadataBehavior);
            }

            host.AddServiceEndpoint(typeof (IMetadataExchange),
                MetadataExchangeBindings.CreateMexTcpBinding(),
                "mex");

            host.Open();
            Console.WriteLine("Started!");
            Console.ReadKey();
            host.Close();
        }
    }
}
