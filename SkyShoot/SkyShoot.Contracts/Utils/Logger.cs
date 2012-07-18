using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using SkyShoot.Contracts.GameEvents;

namespace SkyShoot.Contracts.Utils
{
	public class Logger : TraceListener
	{
		public static Logger ServerLogger;
		public static Logger ClientLogger;

		public static string SolutionPath;

		static Logger()
		{
#if DEBUG
			string currentPath = Environment.CurrentDirectory;
			
			currentPath = currentPath.Replace("\\anyCPU", "");
			currentPath = currentPath.Replace("\\Debug", ""); 
			currentPath = currentPath.Replace("\\bin", "\\..");

			SolutionPath = currentPath;
#endif
		}

		private readonly StreamWriter _streamWriter;
		private readonly TimeHelper _timeHelper;

		public Logger(string filename)
			: this(filename, new TimeHelper())
		{

		}

		public Logger(string filename, TimeHelper timeHelper)
		{
			_streamWriter = File.CreateText(filename);
			_timeHelper = timeHelper;
		}

		public override void WriteLine(string message, string category)
		{
			message = string.Format("[{0}] [{1}]: {2}", category, _timeHelper, message);
			_streamWriter.WriteLine(message);
			_streamWriter.Flush();
		}

		public override void Write(string message)
		{
			_streamWriter.Write(string.Format("[{0}] {1}", _timeHelper, message));
			_streamWriter.Flush();
		}

		public void WriteLine()
		{
			_streamWriter.WriteLine();
			_streamWriter.Flush();
		}

		public override void WriteLine(string message)
		{
			message = string.Format("[{0}] {1}", _timeHelper, message);
			_streamWriter.WriteLine(message);
			_streamWriter.Flush();
		}

		public override void Fail(string message)
		{
			_streamWriter.WriteLine(string.Format("{0}", message));
			_streamWriter.Flush();
		}

		public static void PrintEvents(AGameEvent[] gameEvents)
		{
			var eventsString = new StringBuilder();
			eventsString.Append("Events, count = ");
			eventsString.Append(gameEvents.Length);
			foreach (var gameEvent in gameEvents)
			{
				eventsString.Append("\n  " + gameEvent);
			}
			Trace.WriteLine(eventsString);
		}
	}
}
