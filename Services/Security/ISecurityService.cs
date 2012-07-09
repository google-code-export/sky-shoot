using Services.Base;

namespace Services.Security
{
	/// <summary>
	/// Lists all possible authentication statuses.
	/// </summary>
	public enum AuthenticationStatus
	{
		/// <summary>
		/// Indicates that user has been successfully authenticated in the system.
		/// </summary>
		Success,

		/// <summary>
		/// Indicates that user with the specified credentials was not found.
		/// </summary>
		NotFound,

		/// <summary>
		/// Indicates that user with the specified credentials exists but his account was disabled
		/// by the system administrator.
		/// </summary>
		AccountDisabled,

		/// <summary>
		/// Indicates that user with the specified credentials exists but his account was not
		/// verified.
		/// </summary>
		AccountNotVerified,

		/// <summary>
		/// Indicates that user with the specified login exists but the provided password does not
		/// match.
		/// </summary>
		InvalidPassword
	}

	/// <summary>
	/// Defines interface for authentication and authorization within the system.
	/// </summary>
	public interface ISecurityService : IServiceBase
	{
		/// <summary>
		/// Authenticates user in the system.
		/// </summary>
		/// <param name="login">User login.</param>
		/// <param name="password">User password.</param>
		/// <returns>Authentication result for provided credentials.</returns>
		AuthenticationStatus AuthenticateUser(string login, string password);
	}
}