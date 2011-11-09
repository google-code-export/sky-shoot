using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace ProtoSkyShootWcfService
{
    interface ICallback
    {
        [OperationContract]
        void NotifyClient(string info);
    }
}
