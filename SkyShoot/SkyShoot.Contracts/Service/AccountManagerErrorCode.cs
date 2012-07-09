using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyShoot.Contracts.Service
{
	public enum AccountManagerErrorCode
	{
		Ok,
		UnknownExceptionOccured,
		InvalidUsernameOrPassword,
		UsernameTaken,
		UnknownError //if this value is returned, then AccountManager code must be bugged
	}
}
