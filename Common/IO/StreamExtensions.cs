using System;
using System.IO;

namespace Common.IO
{
	/// <summary>
	/// Contains extension methods for <see cref="Stream"/> class.
	/// </summary>
	public static class StreamExtensions
	{
		/// <summary>
		/// Reads bytes from the specified stream to the end.
		/// </summary>
		/// <param name="stream">The stream to read bytes from.</param>
		/// <returns>Bytes array read from the specified stream.</returns>
		public static byte[] ReadToEnd(this Stream stream)
		{
			byte[] readBuffer = new byte[4096];

			int totalBytesRead = 0;
			int bytesRead;

			while ((bytesRead = stream.Read(readBuffer,
											totalBytesRead,
											readBuffer.Length - totalBytesRead)) > 0)
			{
				totalBytesRead += bytesRead;
				if (totalBytesRead != readBuffer.Length)
					continue;

				int nextByte = stream.ReadByte();
				if (nextByte == -1)
					continue;

				byte[] temp = new byte[readBuffer.Length * 2];
				Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
				Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
				readBuffer = temp;
				totalBytesRead++;
			}

			byte[] buffer = readBuffer;
			if (readBuffer.Length != totalBytesRead)
			{
				buffer = new byte[totalBytesRead];
				Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
			}

			return buffer;
		}
	}
}