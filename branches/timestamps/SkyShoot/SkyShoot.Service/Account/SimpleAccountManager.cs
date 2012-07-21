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

		private static readonly SimpleAccountManager LocalInstance = new SimpleAccountManager();

		public static SimpleAccountManager Instance
		{
			get { return LocalInstance; }
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

		public AccountManagerErrorCode Register(string username, string password/*,  string email*/)
		{
			try
			{
				LocalInstance.ReadAccounts();
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
				LocalInstance.WriteAccounts();
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
				LocalInstance.ReadAccounts();
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
				LocalInstance.ReadAccounts();
				AccountInfo account;
				if (!_accounts.TryGetValue(username, out account) || !account.Password.Equals(password))
				{
					return AccountManagerErrorCode.InvalidUsernameOrPassword;
				}
				_accounts.Remove(username);
				LocalInstance.WriteAccounts();
			}
			catch (Exception)
			{
				return AccountManagerErrorCode.UnknownExceptionOccured;
			}
			return AccountManagerErrorCode.Ok;
		}
	}
}
