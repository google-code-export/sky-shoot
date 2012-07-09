using System.Data;
using System.Diagnostics;
using System.Linq;
using Services.Base;
using Services.Data;

namespace Services.Users
{
	public class UsersService : ServiceBase, IUsersService
	{
		public UsersService(IEntities repository)
			: base(repository)
		{
		}

		/// <summary>
		/// Creates a new user in the system.
		/// </summary>
		/// <param name="user">User to create.</param>
		/// <param name="sendActivationEmail">Indicates whether activation email should be sent to the user.</param>
		public void Create(User user, bool sendActivationEmail = false)
		{
			Debug.Assert(user != null, "user != null");

			if (sendActivationEmail)
			{
				// ToDo: Send activation email
			}

			try
			{
				Repository.Users.AddObject(user);
				Repository.SaveChanges();
			}
			catch (UpdateException ex)
			{
				Trace.TraceError("Unexpected error occurred while trying to save changes in db:", ex);
				throw new ServiceException(ex.Message);
			}
		}

		/// <summary>
		/// Delete user who email equals specified email.
		/// </summary>
		/// <param name="email">The email of the user.</param>
		public void Delete(string email)
		{
			Debug.Assert(email != null, "email != null");

			var user = Repository.Users.FirstOrDefault(x => x.Email == email);
			if (user != null)
			{
				try
				{
					Repository.Users.DeleteObject(user);
					Repository.SaveChanges();
				}
				catch (UpdateException ex)
				{
					Trace.TraceError("Unexpected error occurred while trying to save changes in db:", ex);
					throw new ServiceException(ex.Message);
				}
			}
		}

		/// <summary>
		/// Determines whether email and login availables for registration or is already occupied.
		/// </summary>
		/// <param name="email">Email to check.</param>
		/// <param name="login">The login to check.</param>
		/// <returns>
		///   <c>true</c> if the specified email and login is availables for registration; otherwise, <c>false</c>.
		/// </returns>
		public bool IsEmailAndLoginAvailables(string email, string login)
		{
			Debug.Assert(email != null, "email != null");
			Debug.Assert(login != null, "login != null");

			return Repository.Users.All(x => x.Email != email) &&
				   Repository.Users.All(x => x.Login != login);
		}
	}
}