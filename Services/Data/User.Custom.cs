using System.ComponentModel;

namespace Services.Data
{
	/// <summary>
	/// Lists all user statuses available in the system.
	/// </summary>
	public enum UserStatus : byte
	{
		/// <summary>
		/// Indicates that the registration attempt has been made but user hasn't verified his email yet.
		/// </summary>
		[Description("Not verified")]
		NotVerified = 0,

		/// <summary>
		/// Indicates that user has registered and verified his email.
		/// </summary>
		Active = 1,

		/// <summary>
		/// Indicates that user account has been deactivated and he cannot use the system anymore.
		/// </summary>
		Inactive = 2
	}

	/// <summary>
	/// Custom properties for user entity.
	/// </summary>
	public partial class User
	{
		/// <summary>
		/// Gets or sets the status of the user.
		/// </summary>
		public UserStatus UserStatus
		{
			get { return (UserStatus)Status; }
			set { Status = (byte)value; }
		}
	}
}