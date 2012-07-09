using System.Data;
using System.Diagnostics;
using Services.Data;

namespace Services.Base
{
	/// <summary>
	/// Provides base functionality for entity services.
	/// </summary>
	public abstract class ServiceBase : IServiceBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceBase"/> class.
		/// </summary>
		/// <param name="repository">The entities repository to use for data access.</param>
		internal ServiceBase(IEntities repository)
		{
			Debug.Assert(repository != null, "repository != null");

			Repository = repository;
		}

		/// <summary>
		/// Gets or sets the entities to use for data access.
		/// </summary>
		protected IEntities Repository { get; set; }

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
		/// resources.
		/// </summary>
		public virtual void Dispose()
		{
			Repository.Dispose();
		}

		/// <summary>
		/// Saves the changes in DB.
		/// </summary>
		public void SaveChanges()
		{
			try
			{
				Repository.SaveChanges();
			}
			catch (UpdateException ex)
			{
				Trace.TraceError("Unexpected error occurred while trying to save changes in db:", ex);
				throw new ServiceException(ex.Message);
			}
			
		}
	}
}