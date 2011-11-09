using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using ProtoSkyShootWCFContracts;

namespace ProtoSkyShootWCFClient
{
    class MyCallback:ServiceReference1.IMyServiceCallback
    {
        public void NotifyClient(string info)
        {
            Console.WriteLine(info + " called back!");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var client = new ServiceReference1.MyServiceClient(
                new InstanceContext(new MyCallback()));
            Console.WriteLine(client.GetMyData(new CustomData()
                                                   {
                                                       MyData = "Client",
                                                       
                                                   }));
            Console.ReadKey();
        }
    }
}
