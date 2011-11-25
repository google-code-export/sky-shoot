using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace SkyShoot.Service.Logger
{
	class TableTraceListener : TraceListener
	{
        public TableTraceListener():base()
        {
            var ws = System.IO.File.CreateText("log.txt");
            ws.Close();
        }

		public override void Write(string message)
		{
			System.Console.Write("[" + DateTime.Now + "] INFO: " + message);
		}

		public override void WriteLine(string message)
		{
            var ws = System.IO.File.AppendText("log.txt");
            message = "[" + DateTime.Now + "." + DateTime.Now.Millisecond + "] INFO: " + message;
            ws.WriteLine(message);
            ws.Close();
			System.Console.WriteLine(message);
		}

		public override void Fail(string message)
		{
			System.Console.WriteLine("[" + DateTime.Now + "] ERROR: " + message);
		}
	}
}
