using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ProtoSkyShootWCFContracts
{
    [DataContract]
    public class CustomData
    {
        [DataMember]
        public string MyData { get; set;}

        public int MyNumeric;
    }
}
