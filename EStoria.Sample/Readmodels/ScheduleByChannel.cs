using System;
using System.Collections.Generic;
using System.Linq;

namespace EStoria.Sample.Readmodels
{
	public class ScheduleByChannel : ScheduleBase
	{
		public IEnumerable<IGrouping<string, ScheduledTransmission>> Schedule
		{
			get { return Transmissions.Values.OrderBy(t => t.ChannelId).GroupBy(t => t.ChannelId); }
		}
	}
}