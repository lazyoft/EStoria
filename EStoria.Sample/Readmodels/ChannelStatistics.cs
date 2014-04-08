using System;
using System.Collections.Generic;
using System.Linq;

namespace EStoria.Sample.Readmodels
{
	public class ChannelStatistics
	{
		readonly IDictionary<string, Statistics> _channelStats = new Dictionary<string, Statistics>();
		readonly IDictionary<string, string> _transmissionToChannels = new Dictionary<string, string>();

		public Statistics this[string transmission]
		{
			get { return _channelStats[_transmissionToChannels[transmission]]; }
			set {  _channelStats[_transmissionToChannels[transmission]] = value; }
		}

		public IEnumerable<IGrouping<string, Statistics>> Statistics
		{
			get { return _channelStats.GroupBy(pair => pair.Key, pair => pair.Value); }
		}

		public void AddTransmission(string transmission, string channel)
		{
			_transmissionToChannels[transmission] = channel;
			_channelStats[channel] = new Statistics { Active = 1 };
		}
	}
}