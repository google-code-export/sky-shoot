using System;

namespace SkyShoot.Game
{
#if WINDOWS || XBOX
    static class Program
    {
        static void Main(string[] args)
        {
            using (SkyShootGame game = new SkyShootGame())
            {
                game.Run();
            }
        }
    }
#endif
}

