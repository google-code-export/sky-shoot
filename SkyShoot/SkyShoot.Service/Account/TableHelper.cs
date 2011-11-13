using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System.Data.Services.Client;

namespace SkyShoot.Service.Account
{
    public class TableHelper
    {
        public CloudStorageAccount Account;
        public CloudTableClient TableClient;

        // Constructor - получаем настройки из файла с конфигурацией

        public TableHelper(string configurationSettingName, bool hostedService)
        {
            if (hostedService)
            {
                CloudStorageAccount.SetConfigurationSettingPublisher(
                    (configName, configSettingPublisher) =>
                    {
                        var connectionString = RoleEnvironment.GetConfigurationSettingValue(configName);
                        configSettingPublisher(connectionString);
                    }
                );
            }
            else
            {
                CloudStorageAccount.SetConfigurationSettingPublisher(
                    (configName, configSettingPublisher) =>
                    {
                        var connectionString = ConfigurationManager.ConnectionStrings[configName].ConnectionString;
                        configSettingPublisher(connectionString);
                    }
                );
            }

            Account = CloudStorageAccount.FromConfigurationSetting(configurationSettingName);

            TableClient = Account.CreateCloudTableClient();
            TableClient.RetryPolicy = RetryPolicies.Retry(4, TimeSpan.Zero);
        }

        // Constructor - pass in a storage connection string.

        public TableHelper(string connectionString)
        {
            Account = CloudStorageAccount.Parse(connectionString);

            TableClient = Account.CreateCloudTableClient();
            TableClient.RetryPolicy = RetryPolicies.Retry(4, TimeSpan.Zero);
        }

        public bool CreateTable(string tableName)
        {
            try
            {
                TableClient.CreateTable(tableName);
                return true;
            }
            catch (StorageClientException ex)
            {
                if ((int)ex.StatusCode == 409)
                {
                    return false;
                }

                throw;
            }
        }

        public bool InsertEntity(string tableName, object obj)
        {
            try
            {
                TableServiceContext tableServiceContext = TableClient.GetDataServiceContext();

                tableServiceContext.AddObject(tableName, obj);
                tableServiceContext.SaveChanges();

                return true;
            }
            catch (DataServiceRequestException)
            {
                return false;
            }
            catch (StorageClientException ex)
            {
                if ((int)ex.StatusCode == 404)
                {
                    return false;
                }

                throw;
            }
        }

        public bool GetEntity<T>(string tableName, string partitionKey, string rowKey, out T entity) where T : TableServiceEntity
        {
            entity = null;

            try
            {
                TableServiceContext tableServiceContext = TableClient.GetDataServiceContext();
                IQueryable<T> entities = (from e in tableServiceContext.CreateQuery<T>(tableName)
                                          where e.PartitionKey == partitionKey && e.RowKey == rowKey
                                          select e);

                entity = entities.FirstOrDefault();

                return true;
            }
            catch (DataServiceRequestException)
            {
                return false;
            }
            catch (StorageClientException ex)
            {
                if ((int)ex.StatusCode == 404)
                {
                    return false;
                }

                throw;
            }
        }

        public bool ReplaceUpdateEntity<T>(string tableName, string partitionKey, string rowKey, T obj) where T : TableServiceEntity
        {
            try
            {
                TableServiceContext tableServiceContext = TableClient.GetDataServiceContext();
                IQueryable<T> entities = (from e in tableServiceContext.CreateQuery<T>(tableName)
                                          where e.PartitionKey == partitionKey && e.RowKey == rowKey
                                          select e);

                T entity = entities.FirstOrDefault();

                Type t = obj.GetType();
                PropertyInfo[] pi = t.GetProperties();

                foreach (PropertyInfo p in pi)
                {
                    p.SetValue(entity, p.GetValue(obj, null), null);
                }

                tableServiceContext.UpdateObject(entity);
                tableServiceContext.SaveChanges(SaveChangesOptions.ReplaceOnUpdate);

                return true;
            }
            catch (DataServiceRequestException)
            {
                return false;
            }
            catch (StorageClientException ex)
            {
                if ((int)ex.StatusCode == 404)
                {
                    return false;
                }

                throw;
            }
        }


        public bool DeleteEntity<T>(string tableName, string partitionKey, string rowKey) where T : TableServiceEntity
        {
            try
            {
                TableServiceContext tableServiceContext = TableClient.GetDataServiceContext();
                IQueryable<T> entities = (from e in tableServiceContext.CreateQuery<T>(tableName)
                                          where e.PartitionKey == partitionKey && e.RowKey == rowKey
                                          select e);

                T entity = entities.FirstOrDefault();

                if (entities != null)
                {
                    tableServiceContext.DeleteObject(entity);
                    tableServiceContext.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (DataServiceRequestException)
            {
                return false;
            }
            catch (StorageClientException ex)
            {
                if ((int)ex.StatusCode == 404)
                {
                    return false;
                }

                throw;
            }
        }

        public static string GetRandomString()
        {
            string path = Path.GetRandomFileName();
            path = path.Replace(".", "");
            return path;
        }

    }
}