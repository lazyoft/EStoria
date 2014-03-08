using System;

namespace EStoria.ValueObjects
{
	public class EventInfo : IEquatable<EventInfo>
	{
		public int Serial { get; private set; }
		public string StreamName { get; private set; }

		public EventInfo(int serial, string streamName)
		{
			Serial = serial;
			StreamName = streamName;
		}

		public bool Equals(EventInfo other)
		{
			if(ReferenceEquals(null, other)) return false;
			if(ReferenceEquals(this, other)) return true;
			return Serial == other.Serial && string.Equals(StreamName, other.StreamName);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (Serial * 397) ^ StreamName.GetHashCode();
			}
		}

		public static bool operator ==(EventInfo left, EventInfo right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(EventInfo left, EventInfo right)
		{
			return !Equals(left, right);
		}

		public static bool operator >(EventInfo left, EventInfo right)
		{
			return left.Serial > right.Serial;
		}

		public static bool operator <(EventInfo left, EventInfo right)
		{
			return left.Serial < right.Serial;
		}

		public static bool operator >=(EventInfo left, EventInfo right)
		{
			return left.Serial >= right.Serial;
		}

		public static bool operator <=(EventInfo left, EventInfo right)
		{
			return left.Serial <= right.Serial;
		}
		
		public override bool Equals(object obj)
		{
			if(ReferenceEquals(null, obj)) return false;
			if(ReferenceEquals(this, obj)) return true;
			return obj.GetType() == GetType() && Equals((EventInfo)obj);
		}
	}
}