using System;
using System.Diagnostics;
using System.IO;

namespace SkyShoot.Service.Logger
{
	class TableTraceListener : TraceListener
	{
		readonly StreamWriter _ws;

		public TableTraceListener()
		{
			_ws = File.CreateText("log.txt");
		}

		public override void WriteLine(string message, string category)
		{
			message = string.Format("[{0}.{1}] {2}: {3}", DateTime.Now, DateTime.Now.Millisecond, category, message);
			_ws.WriteLine(message);
			_ws.Flush();

			//ws.Close();
			//System.Console.WriteLine(message);
		}

		public override void Write(string message)
		{
			Console.Write(string.Format("[INFO] [{0}] {1}", DateTime.Now, message));
		}

		public override void WriteLine(string message)
		{
			message = string.Format("[INFO] [{0}:{1}] {2}", DateTime.Now, DateTime.Now.Millisecond, message);
			_ws.WriteLine(message);
			_ws.Flush();
			//System.Console.WriteLine(message);
		}

		public override void Fail(string message)
		{
			Console.WriteLine(string.Format("[{0}] ERROR: {1}", DateTime.Now, message));
		}
	}
}
