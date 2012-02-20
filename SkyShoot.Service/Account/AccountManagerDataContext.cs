using System.Linq;

namespace SkyShoot.Service.Account
{
    public class AccountManagerDataContext
        : Microsoft.WindowsAzure.StorageClient.TableServiceContext
    {
        public AccountManagerDataContext(string baseAddress, Microsoft.WindowsAzure.StorageCredentials credentials)
            : base(baseAddress, credentials)
        { }

        public IQueryable<AccountManagerEntry> AccountManagerEntry
        {
            get
            {
                return this.CreateQuery<AccountManagerEntry>("AccountManagerEntry");
            }
        }
    }
}
