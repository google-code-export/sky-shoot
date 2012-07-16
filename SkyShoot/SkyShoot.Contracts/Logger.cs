using System;
using System.IO;

namespace SkyShoot.Contracts
{
	public class Logger
	{
		private readonly StreamWriter _streamWriter;

		public Logger(string filename)
		{
			_streamWriter = File.CreateText(filename);
		}

		public void WriteLine(string message, string category)
		{
			message = string.Format("[{0}.{1}] {2}: {3}", DateTime.Now, DateTime.Now.Millisecond, category, message);
			_streamWriter.WriteLine(message);
			_streamWriter.Flush();
		}

		public void Write(string message)
		{
			_streamWriter.Write(string.Format("[{0}] INFO: {1}", DateTime.Now, message));
			_streamWriter.Flush();
		}

		public void WriteLine()
		{
			_streamWriter.WriteLine();
			_streamWriter.Flush();
		}

		public void WriteLine(string message)
		{
			message = string.Format("[INFO] [{0}:{1}] {2}", DateTime.Now, DateTime.Now.Millisecond, message);
			_streamWriter.WriteLine(message);
			_streamWriter.Flush();
		}

		public void Fail(string message)
		{
			_streamWriter.WriteLine(string.Format("[{0}] ERROR: {1}", DateTime.Now, message));
			_streamWriter.Flush();
		}
	}
}
