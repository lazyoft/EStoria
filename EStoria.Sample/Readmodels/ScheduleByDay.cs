using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace EStoria.Sample.Readmodels
{
	public class ScheduleByDay : ScheduleBase
	{
		[JsonIgnore]
		public IEnumerable<IGrouping<DateTime, ScheduledTransmission>> Schedule
		{
			get { return Transmissions.Values.OrderBy(t => t.Start).GroupBy(t => t.Start.Date); }
		}
	}
}