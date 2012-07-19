using System;

namespace SkyShoot.Contracts.Utils
{
	public class TimeHelper
	{
		private readonly long _startTime;

		public TimeHelper(long startTime)
		{
			_startTime = startTime;
		}

		public TimeHelper()
		{
			_startTime = 0;
		}

		/// <summary>
		/// Определение времени в миллисекундах,
		/// прошедшего с start time
		/// </summary>
		/// <returns>количество миллисекунд, прошедшее с _startTime</returns>
		public long GetTime()
		{
			return NowMilliseconds - _startTime;
		}

		public override string ToString()
		{
			var dateTime = new DateTime(GetTime() * 10000);
			return String.Format("{0:H:m:s:fff}", dateTime);
		}

		/// <summary>
		/// Текущее количество миллисекунд
		/// </summary>
		public static long NowMilliseconds
		{
			get { return DateTime.Now.Ticks / 10000; }
		}
	}
}
