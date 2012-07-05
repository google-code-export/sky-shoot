using Services.Base;
using Services.Data;

namespace Services.Users
{
	public interface IUsersService : IServiceBase
	{
		/// <summary>
		/// Creates a new user in the system.
		/// </summary>
		/// <param name="user">User to create.</param>
		/// <param name="sendActivationEmail">
		/// Indicates whether activation email should be sent to the user.
		/// </param>
		void Create(User user, bool sendActivationEmail = false);

		/// <summary>
		/// Delete user who email equals specified email.
		/// </summary>
		/// <param name="email">The email of the user.</param>
		void Delete(string email);

		/// <summary>
		/// Determines whether email and login availables for registration or is already occupied.
		/// </summary>
		/// <param name="email">Email to check.</param>
		/// <param name="login">The login to check.</param>
		/// <returns>
		///   <c>true</c> if the specified email and login is availables for registration; otherwise, <c>false</c>.
		/// </returns>
		bool IsEmailAndLoginAvailables(string email, string login);
	}
}