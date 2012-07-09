using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkyShoot.Service.Logger
{
    public class Record : Microsoft.WindowsAzure.StorageClient.TableServiceEntity
    {
        public Record()
        {
            PartitionKey = "Records";
            RowKey = string.Format("{0:10}_{1}", DateTime.MaxValue.Ticks - DateTime.Now.Ticks, Guid.NewGuid());
        }

        public string Message { get; set; }

        public string Level { get; set; }
    }
}