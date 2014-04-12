using System;
using System.Collections.Generic;

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

		public IDictionary<string, Statistics> Statistics
		{
			get { return _channelStats; }
		}

		public IDictionary<string, string> TransmissionToChannels { get {  return _transmissionToChannels; } }

		public void AddTransmission(string transmission, string channel)
		{
			_transmissionToChannels[transmission] = channel;
			if(!_channelStats.ContainsKey(channel))
				_channelStats[channel] = new Statistics();
			_channelStats[channel].Active++;
		}
	}
}