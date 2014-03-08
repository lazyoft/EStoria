using System;
using EStoria.Interfaces;

namespace EStoria.Services
{
	public class SystemClock : IClock
	{
		public DateTime Now()
		{
			return DateTime.Now;
		}
	}
}