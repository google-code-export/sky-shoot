using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace SkyShoot.Service.Account
{
	public class AccountManagerDataSource
	{
		private static CloudStorageAccount storageAccount;
		private AccountManagerDataContext context;

		static AccountManagerDataSource()
		{
			storageAccount = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");

			CloudTableClient.CreateTablesFromModel(
				typeof(AccountManagerDataContext),
				storageAccount.TableEndpoint.AbsoluteUri,
				storageAccount.Credentials);
		}

		public AccountManagerDataSource()
		{
			this.context = new AccountManagerDataContext(storageAccount.TableEndpoint.AbsoluteUri, storageAccount.Credentials);
			this.context.RetryPolicy = RetryPolicies.Retry(3, TimeSpan.FromSeconds(1));
		}

		public IEnumerable<AccountManagerEntry> GetAccountManagerEntries()
		{
			var results = from g in this.context.AccountManagerEntry
						  where g.PartitionKey == DateTime.UtcNow.ToString("MMddyyyy")
						  select g;
			return results;
		}

		public bool AddAccountManagerEntry(AccountManagerEntry newItem)
		{
			//var results = from g in this.context.AccountManagerEntry
			//			  where g.Account == newItem.Account
			//			  select g;
			//var entry = results.FirstOrDefault<AccountManagerEntry>();

			var results = (from g in this.context.AccountManagerEntry
						   select g).ToList();

			var entry = results.Where(g=> g.Account == newItem.Account).FirstOrDefault<AccountManagerEntry>();

			if (entry != null)
			{
				return false; // Аккаунт с таким именем уже существует!
			}
			else
			{
				this.context.AddObject("AccountManagerEntry", newItem);
				this.context.SaveChanges();
				return true; // Регистрация прошла
			}
		}

		public bool CheckAccountManagerEntry(string user_name, string password)
		{
			var results = (from g in this.context.AccountManagerEntry
						   select g).ToList();

			var entry = results.Where(g => g.Account == user_name).FirstOrDefault<AccountManagerEntry>();

			if (entry != null)
			{
				if (HashHelper.verifyMd5Hash((password + entry.Salt), entry.HashPassword))
				{
					return true; // Пароль верный
				}
				else
				{
					return false;//Неверный пароль
				}
			}
			else
			{
				return false;
				// Такой аккаунт не зарегестрирован.
			}
		}

		public bool CreateAccountPassword(string user_name, string old_password, string new_password)
		{
			var results = (from g in this.context.AccountManagerEntry
						   select g).ToList();

			var entry = results.Where(g => g.Account == user_name).FirstOrDefault<AccountManagerEntry>();

			if (entry != null)
			{
				if (HashHelper.verifyMd5Hash((old_password + entry.Salt), entry.HashPassword))
				{
					string salt = HashHelper.GetRandomString();
					entry.Salt = salt;
					entry.HashPassword = HashHelper.GetMd5Hash(new_password + salt);
					this.context.UpdateObject(entry);
					this.context.SaveChanges();
					return true; // Замена пароля прошла успешно
				}
				else
				{
					return false;//Неверный пароль
				}
			}
			else
			{
				return false;
				// Такой аккаунт не зарегестрирован.
			}
		}

		public bool DeleteAccountManagerEntry(string user_name)
		{
			var results = (from g in this.context.AccountManagerEntry
						   select g).ToList();

			var entry = results.Where(g => g.Account == user_name).FirstOrDefault<AccountManagerEntry>();

			if (entry != null)
			{
				this.context.DeleteObject(entry);
				this.context.SaveChanges();
				return true; // Удалили аккаунт!

			}
			else
			{
				return false; // Нет такого аккаунта
			}
		}
	}
}
