using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace SkyShoot.Service.Logger
{
	class TableTraceListener : TraceListener
	{
		public override void Write(string message)
		{
			System.Console.Write("[" + DateTime.Now + "] INFO: " + message);
		}

		public override void WriteLine(string message)
		{
			System.Console.WriteLine("[" + DateTime.Now + "] INFO: " + message);
		}

		public override void Fail(string message)
		{
			System.Console.WriteLine("[" + DateTime.Now + "] ERROR: " + message);
		}
	}
}
