using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SkyShoot.Service.Account;
using System.Diagnostics;
using System.Reflection;

using SkyShoot.Contracts.Service;

namespace SkyShoot.ServProgram.Account
{
	public class SimpleAccountManager : IAccountManager
	{
		private const string _fileName = "./accounts";
		private IDictionary<string, AccountInfo> _accounts;

		private static SimpleAccountManager _localInstance = new SimpleAccountManager();

		public static SimpleAccountManager Instance
        {
            get { return _localInstance; }
        }

		private void readAccounts()
		{
			Stream inputStream = System.IO.File.Exists(_fileName) ? File.OpenRead(_fileName) : File.Create(_fileName);
			if (inputStream.Length > 0)
			{
				var deserializer = new BinaryFormatter();
				_accounts = deserializer.Deserialize(inputStream) as Dictionary<string, AccountInfo>;
			}
			else
			{
				_accounts = new Dictionary<string, AccountInfo>();
			}
			inputStream.Close();
		}

		private void writeAccounts()
		{
			var serializer = new BinaryFormatter();
			Stream outputStream = File.OpenWrite(_fileName);
			serializer.Serialize(outputStream, _accounts);
			outputStream.Close();
		}

		public AccountManagerErrorCode Register(string username, string password/*,  string email*/)
		{
			try
			{
				_localInstance.readAccounts();
				if (_accounts.ContainsKey(username))
				{
					return AccountManagerErrorCode.UsernameTaken;
				}
				AccountInfo newAccount = new AccountInfo() {
					Login = username,
					Password = password,
					Email = "--",
					Info = "--"
				};
				_accounts.Add(username, newAccount);
				_localInstance.writeAccounts();
			}
			catch (Exception)
			{
				return AccountManagerErrorCode.UnknownExceptionOccured;
			}
			return AccountManagerErrorCode.Ok;
		}

		public AccountManagerErrorCode Login(string username, string password)
		{
			try
			{
				_localInstance.readAccounts();
				AccountInfo account;
				if (!_accounts.TryGetValue(username, out account) || !account.Password.Equals(password))
				{
					return AccountManagerErrorCode.InvalidUsernameOrPassword;
				}
			}
			catch (Exception)
			{
				return AccountManagerErrorCode.UnknownExceptionOccured;
			}
			return AccountManagerErrorCode.Ok;
		}

		public AccountManagerErrorCode DeleteAccount(string username, string password)
		{
			try
			{
				_localInstance.readAccounts();
				AccountInfo account;
				if (!_accounts.TryGetValue(username, out account) || !account.Password.Equals(password))
				{
					return AccountManagerErrorCode.InvalidUsernameOrPassword;
				}
				_accounts.Remove(username);
				_localInstance.writeAccounts();
			}
			catch (Exception)
			{
				return AccountManagerErrorCode.UnknownExceptionOccured;
			}
			return AccountManagerErrorCode.Ok;
		}


	}
}
