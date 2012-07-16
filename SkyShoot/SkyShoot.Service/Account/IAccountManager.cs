using SkyShoot.Contracts.Service;

namespace SkyShoot.ServProgram.Account
{
	public interface IAccountManager
	{
		AccountManagerErrorCode Register(string username, string password);

		AccountManagerErrorCode Login(string username, string password);

		AccountManagerErrorCode DeleteAccount(string username, string password);
	}
}
