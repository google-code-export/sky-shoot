using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using ProtoSkyShootWCFContracts;

namespace ProtoSkyShootWcfService
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple,InstanceContextMode=InstanceContextMode.Single)]
    class MyService:IMyService
    {
        public string GetMyData(CustomData data)
        {
            var callback = OperationContext.Current.GetCallbackChannel<ICallback>();
            callback.NotifyClient(data.MyData);
            Console.WriteLine(data.MyData + " arrived!");
            return "Hello, " + data.MyData + "!";
        }
    }
}
