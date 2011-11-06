using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure;

namespace SkyShoot.Service.Logger
{
    class DataServiceContext : TableServiceContext
    {
        public DataServiceContext(string baseAddress, StorageCredentials credentials)
            : base(baseAddress, credentials)
        {

        }

        public void AddRecord(string message, string level)
        {
            this.AddObject("Records", new Record { Message = message, Level = level });
            this.SaveChanges();
        }

    }
}