using System;

namespace Services.Base
{
	public interface IServiceBase : IDisposable
	{
		/// <summary>
		/// Saves the changes in DB.
		/// </summary>
		void SaveChanges();
	}
}