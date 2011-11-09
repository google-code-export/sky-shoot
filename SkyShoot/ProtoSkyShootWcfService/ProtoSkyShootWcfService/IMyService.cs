using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using ProtoSkyShootWCFContracts;

namespace ProtoSkyShootWcfService
{
    [ServiceContract(CallbackContract = typeof(ICallback))]
    interface IMyService
    {
        [OperationContract]
        string GetMyData(CustomData data);
    }
}
