using System;

namespace EStoria.ValueObjects
{
	public class CommittedEvent : IEquatable<CommittedEvent>
	{
		public int Serial { get; private set; }
		public string StreamName { get; private set; }
		public DateTime TimeStamp { get; private set; }
		public object Event { get; private set; }

		public CommittedEvent(int serial, string streamName, DateTime timeStamp, object @event)
		{
			Serial = serial;
			StreamName = streamName;
			TimeStamp = timeStamp;
			Event = @event;
		}

		public bool Equals(CommittedEvent other)
		{
			if(ReferenceEquals(null, other)) return false;
			if(ReferenceEquals(this, other)) return true;
			return Serial == other.Serial;
		}

		public override int GetHashCode()
		{
			return Serial;
		}

		public static bool operator ==(CommittedEvent left, CommittedEvent right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(CommittedEvent left, CommittedEvent right)
		{
			return !Equals(left, right);
		}

		public static bool operator >(CommittedEvent left, CommittedEvent right)
		{
			return left.Serial > right.Serial;
		}

		public static bool operator <(CommittedEvent left, CommittedEvent right)
		{
			return left.Serial < right.Serial;
		}

		public static bool operator >=(CommittedEvent left, CommittedEvent right)
		{
			return left.Serial >= right.Serial;
		}

		public static bool operator <=(CommittedEvent left, CommittedEvent right)
		{
			return left.Serial <= right.Serial;
		}
		
		public override bool Equals(object obj)
		{
			if(ReferenceEquals(null, obj)) return false;
			if(ReferenceEquals(this, obj)) return true;
			return obj.GetType() == GetType() && Equals((CommittedEvent)obj);
		}
	}
}