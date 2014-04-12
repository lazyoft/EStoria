using System;
using System.Collections.Generic;

namespace EStoria.Sample.Readmodels
{
	public class ScheduleBase
	{
		readonly IDictionary<string, ScheduledTransmission> _transmissions = new Dictionary<string, ScheduledTransmission>();

		public IDictionary<string, ScheduledTransmission> Transmissions { get {  return _transmissions; } }

		public ScheduledTransmission this[string index]
		{
			get { return _transmissions[index]; }
			set { _transmissions[index] = value; }
		}

		public void Remove(string index)
		{
			_transmissions.Remove(index);
		}
	}
}