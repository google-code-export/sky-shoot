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
				Logger.ClientLogger = new Logger(Logger.SolutionPath + "\\logs\\client_log.txt");
#else
				Logger.ClientLogger = new Logger(@"client_log.txt");
#endif
				Trace.Listeners.Add(Logger.ClientLogger);
				game.Run();
			}
		}
	}
#endif
}

