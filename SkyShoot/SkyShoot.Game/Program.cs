namespace SkyShoot.Game
{
#if WINDOWS || XBOX
    internal static class Program
    {
        private static void Main(string[] args)
        {
            using (var game = new SkyShootGame())
            {
                game.Run();
            }
        }
    }
#endif
}

