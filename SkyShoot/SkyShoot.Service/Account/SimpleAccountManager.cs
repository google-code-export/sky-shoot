using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SkyShoot.Contracts.Service;
using SkyShoot.Service.Account;

namespace SkyShoot.ServProgram.Account
{
	public class SimpleAccountManager : IAccountManager
	{
		private const string FILE_NAME = "./accounts";
		private IDictionary<string, AccountInfo> _accounts;

		private static SimpleAccountManager LocalInstance = null;
		private HashSet<string> _usersOnline;
		private static Object _lock = new Object();

		protected SimpleAccountManager() {
			_usersOnline = new HashSet<string>();
		}

		public static SimpleAccountManager Instance
		{
			get
			{
				if (LocalInstance == null) {
					lock (_lock)
					{
						if (LocalInstance == null)
							LocalInstance = new SimpleAccountManager();
					}
				}
				return LocalInstance;
			}
		}

		private void ReadAccounts()
		{
			Stream inputStream = File.Exists(FILE_NAME) ? File.OpenRead(FILE_NAME) : File.Create(FILE_NAME);
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

		private void WriteAccounts()
		{
			var serializer = new BinaryFormatter();
			Stream outputStream = File.OpenWrite(FILE_NAME);
			serializer.Serialize(outputStream, _accounts);
			outputStream.Close();
		}

		public AccountManagerErrorCode Register(string username, string password)
		{
			lock (_lock)
			{
				try
				{
					ReadAccounts();
					if (_accounts.ContainsKey(username))
					{
						return AccountManagerErrorCode.UsernameTaken;
					}
					var newAccount = new AccountInfo
									 {
										 Login = username,
										 Password = password,
										 Email = "--",
										 Info = "--"
									 };
					_accounts.Add(username, newAccount);
					WriteAccounts();
				}
				catch (Exception)
				{
					return AccountManagerErrorCode.UnknownExceptionOccured;
				}
			}
			return AccountManagerErrorCode.Ok;
		}

		public AccountManagerErrorCode Login(string username, string password)
		{
			lock (_lock)
			{
				try
				{
					ReadAccounts();
					AccountInfo account;
					if (!_accounts.TryGetValue(username, out account) || !account.Password.Equals(password))
					{
						return AccountManagerErrorCode.InvalidUsernameOrPassword;
					}
					if (_usersOnline.Contains(username))
					{
						return AccountManagerErrorCode.UserIsAlreadyOnline;
					}
					WriteAccounts();
					_usersOnline.Add(username);
				}
				catch (Exception)
				{
					return AccountManagerErrorCode.UnknownExceptionOccured;
				}
			}
			return AccountManagerErrorCode.Ok;
		}

		public AccountManagerErrorCode Logout(string username)
		{
			lock (_lock)
			{
				try
				{
					ReadAccounts();
					AccountInfo account;
					if (!_accounts.TryGetValue(username, out account))
					{
						return AccountManagerErrorCode.InvalidUsernameOrPassword;
					}
					if (!_usersOnline.Contains(username))
					{
						return AccountManagerErrorCode.UserIsAlreadyOffline;
					}
					_usersOnline.Remove(username);
					WriteAccounts();
				}
				catch (Exception)
				{
					return AccountManagerErrorCode.UnknownExceptionOccured;
				}
			}
			return AccountManagerErrorCode.Ok;
		}
		public AccountManagerErrorCode DeleteAccount(string username, string password)
		{
			lock (_lock)
			{
				try
				{
					ReadAccounts();
					AccountInfo account;
					if (!_accounts.TryGetValue(username, out account) || !account.Password.Equals(password))
					{
						return AccountManagerErrorCode.InvalidUsernameOrPassword;
					}
					_accounts.Remove(username);
					WriteAccounts();
				}
				catch (Exception)
				{
					return AccountManagerErrorCode.UnknownExceptionOccured;
				}
			}
			return AccountManagerErrorCode.Ok;
		}
	}
}
