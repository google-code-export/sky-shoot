using System.Diagnostics;

using System.Data.Services.Client;

using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;

namespace SkyShoot.Service.Logger
{
    public class TableTraceListener : TraceListener
    {
        CloudStorageAccount account;
        DataServiceContext context;

        public TableTraceListener()
        {
            Microsoft.WindowsAzure.CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
            {
                configSetter(RoleEnvironment.GetConfigurationSettingValue(configName));
            });
            account = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
            context = new DataServiceContext(account.TableEndpoint.ToString(), account.Credentials);

            CloudTableClient.CreateTablesFromModel(typeof(DataServiceContext),
                                    account.TableEndpoint.AbsoluteUri, account.Credentials);
        }

        public override void Write(string  message)
        {
            try
            {
                context.AddRecord(message, "info");
            }
            catch (DataServiceRequestException)
            {

            }
        }

        public override void WriteLine(string message)
        {
            Write(message + System.Environment.NewLine);
        }

        public override void Fail(string message)
        {
            try
            {
                context.AddRecord(message, "error");
            }
            catch (DataServiceRequestException)
            {

            }
        }
    }
}