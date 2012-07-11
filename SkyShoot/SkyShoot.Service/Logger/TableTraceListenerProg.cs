using System;
using System.Diagnostics;
using System.IO;

namespace SkyShoot.Service.Logger
{
	class TableTraceListener : TraceListener
	{
		StreamWriter ws;

	    public TableTraceListener()
	    {
	        ws = File.CreateText("log.txt");
        }

		public override void WriteLine(string message, string category)
		{
		    message = string.Format("[{0}.{1}] {2}: {3}", DateTime.Now, DateTime.Now.Millisecond, category, message);
			ws.WriteLine(message);
			ws.Flush();
			
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
			ws.WriteLine(message);
			ws.Flush();
			//System.Console.WriteLine(message);
		}

		public override void Fail(string message)
		{
            Console.WriteLine(string.Format("[{0}] ERROR: {1}", DateTime.Now, message));
		}
	}
}
