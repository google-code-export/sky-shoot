namespace TestArea
{
	public abstract class AWeapon : IWeapon
	{
		public virtual string TexturePath
		{
			get
			{
				return "Default Path";
			}
		}

		public virtual int CountBullets
		{
			get
			{
				return 1;
			}
		}
	}

	public interface IWeapon
	{
		string TexturePath { get; }

		int CountBullets { get; }
	}
}