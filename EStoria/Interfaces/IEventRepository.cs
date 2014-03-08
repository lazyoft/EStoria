using System;
using System.Collections.Generic;
using EStoria.ValueObjects;

namespace EStoria.Interfaces
{
	public interface IEventRepository
	{
		void Save(EventInfo info, byte[] data);
		IEnumerable<EventInfo> GetInfos();
		byte[] Read(EventInfo info);
	}
}