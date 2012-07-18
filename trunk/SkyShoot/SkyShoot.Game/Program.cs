using System.Diagnostics;
using SkyShoot.Contracts.Utils;

namespace SkyShoot.Game
{
#if WINDOWS || XBOX
	internal static class Program
	{
		private static void Main()
		{
			using (var game = new SkyShootGame())
			{
#if DEBUG
				Trace.Listeners.Add(new Logger(@"..\..\..\..\logs\client_log.txt"));
#else
				Trace.Listeners.Add(new Logger(@"client_log.txt"));
#endif
				game.Run();
			}
		}
	}
#endif
}

