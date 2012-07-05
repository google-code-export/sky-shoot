using System.Diagnostics;
using System.Linq;
using Common.Security;
using Services.Base;
using Services.Data;

namespace Services.Security
{
	public class SecurityService : ServiceBase, ISecurityService
	{
		public SecurityService(IEntities repository)
			: base(repository)
		{
		}

		/// <summary>
		/// Authenticates user in the system.
		/// </summary>
		/// <param name="login">User login.</param>
		/// <param name="password">User password.</param>
		/// <returns>
		/// Authentication result for provided credentials.
		/// </returns>
		public AuthenticationStatus AuthenticateUser(string login, string password)
		{
			Debug.Assert(login != null, "login != null");
			Debug.Assert(password != null, "password != null");

			User user = Repository.Users.FirstOrDefault(u => u.Email == login);
			if (null == user)
				return AuthenticationStatus.NotFound;

			if (!string.IsNullOrEmpty(password))
			{
				// We use registration date as a salt in order to avoid extra field and increase security
				byte[] salt = user.Salt;
				byte[] encryptedPassword = Cryptographer.GenerateHash(password, salt);

				if (!encryptedPassword.SequenceEqual(user.Password))
				{
					Trace.TraceWarning("Authentication failure: invalid password for user {0}", login);
					return AuthenticationStatus.InvalidPassword;
				}
			}

			if (UserStatus.NotVerified == user.UserStatus)
			{
				Trace.TraceWarning("Authentication failure: account {0} is not verified", login);
				return AuthenticationStatus.AccountNotVerified;
			}

			if (UserStatus.Inactive == user.UserStatus)
			{
				Trace.TraceWarning("Authentication failure: account {0} is disabled", login);
				return AuthenticationStatus.AccountDisabled;
			}

			Trace.TraceInformation("User {0} has been successfully authenticated", login);

			return AuthenticationStatus.Success;
		}
	}
}