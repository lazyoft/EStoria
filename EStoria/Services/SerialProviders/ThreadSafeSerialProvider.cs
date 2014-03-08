using System;
using System.Threading;
using EStoria.Interfaces;

namespace EStoria.Services.SerialProviders
{
	public class ThreadSafeSerialProvider : ISerialProvider
	{
		int _next;

		public ThreadSafeSerialProvider(int start = 0)
		{
			_next = start;
		}

		public int Next()
		{
			return Interlocked.Increment(ref _next);
		}
	}
}