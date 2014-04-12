using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace EStoria.Sample.Readmodels
{
	public class ScheduleByChannel : ScheduleBase
	{
		[JsonIgnore]
		public IEnumerable<IGrouping<string, ScheduledTransmission>> Schedule
		{
			get { return Transmissions.Values.OrderBy(t => t.ChannelId).GroupBy(t => t.ChannelId); }
		}
	}
}