using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace SkyShoot.Service.Logger
{
	class TableTraceListener : TraceListener
	{
		System.IO.StreamWriter ws;
        public TableTraceListener():base()
        {
			ws = System.IO.File.CreateText("log.txt");
        }

		public override void WriteLine(string message, string category)
		{
			
			message = "[" + DateTime.Now + "." + DateTime.Now.Millisecond + "] "+category+": " + message;
			ws.WriteLine(message);
			ws.Flush();
			
			//ws.Close();
			//System.Console.WriteLine(message);
		}

		public override void Write(string message)
		{
			System.Console.Write("[" + DateTime.Now + "] INFO: " + message);
		}

		public override void WriteLine(string message)
		{
            message = "[" + DateTime.Now + "." + DateTime.Now.Millisecond + "] INFO: " + message;
			ws.WriteLine(message);
			ws.Flush();
			//System.Console.WriteLine(message);
		}

		public override void Fail(string message)
		{
			System.Console.WriteLine("[" + DateTime.Now + "] ERROR: " + message);
		}
	}
}
