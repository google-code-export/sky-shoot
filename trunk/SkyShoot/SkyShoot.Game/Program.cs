namespace SkyShoot.Game
{
#if WINDOWS || XBOX
	internal static class Program
	{
		private static void Main()
		{
			using (var game = new SkyShootGame())
			{
				game.Run();
			}
		}
	}
#endif
}

