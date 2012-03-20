using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SkyShoot.Service.Account
{
	public class TestPattern
	{
		public static bool TestAccountName(string user_name)
		{
			Regex regex = new Regex("[^a-zA-Z_-.]");
			bool f = regex.IsMatch(user_name);
			if (f)
				return false;
			else
				return true;
		}

		public static bool TestPassword(string password)
		{
			Regex regex = new Regex("[^0-9a-zA-Z!@#$%^&*()_+:;,.-]");
			bool f = regex.IsMatch(password);
			if (f)
				return false;
			else
				return true;
		}
	}
}
