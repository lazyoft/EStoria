using System;

namespace EStoria
{
	public abstract class BaseDisposable : IDisposable
	{
		public bool Disposed { get; private set; }

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			Disposed = true;
		}

		~BaseDisposable()
		{
			Dispose(false);
		}
	}
}