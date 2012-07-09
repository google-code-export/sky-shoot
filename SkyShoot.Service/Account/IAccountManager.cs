using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyShoot.ServProgram.Account
{
	public interface IAccountManager
	{
		bool Register(string username, string password);

		bool Login(string username, string password);

		bool DeleteAccount(string username, string password);

	}
}
