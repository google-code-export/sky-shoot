using System.Diagnostics;
using System.IO;

namespace SkyShoot.Contracts.Utils
{
	public class Logger : TraceListener
	{
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
	}
}
