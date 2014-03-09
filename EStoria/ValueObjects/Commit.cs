using System;

namespace EStoria.ValueObjects
{
	public class Commit : IEquatable<Commit>
	{
		public int Serial { get; private set; }
		public string Name { get; private set; }
		public DateTime TimeStamp { get; private set; }
		public object Data { get; private set; }

		public Commit(int serial, string name, DateTime timeStamp, object data)
		{
			Serial = serial;
			Name = name;
			TimeStamp = timeStamp;
			Data = data;
		}

		public bool Equals(Commit other)
		{
			if(ReferenceEquals(null, other)) return false;
			if(ReferenceEquals(this, other)) return true;
			return Serial == other.Serial;
		}

		public override int GetHashCode()
		{
			return Serial;
		}

		public static bool operator ==(Commit left, Commit right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Commit left, Commit right)
		{
			return !Equals(left, right);
		}

		public static bool operator >(Commit left, Commit right)
		{
			return left.Serial > right.Serial;
		}

		public static bool operator <(Commit left, Commit right)
		{
			return left.Serial < right.Serial;
		}

		public static bool operator >=(Commit left, Commit right)
		{
			return left.Serial >= right.Serial;
		}

		public static bool operator <=(Commit left, Commit right)
		{
			return left.Serial <= right.Serial;
		}
		
		public override bool Equals(object obj)
		{
			if(ReferenceEquals(null, obj)) return false;
			if(ReferenceEquals(this, obj)) return true;
			return obj.GetType() == GetType() && Equals((Commit)obj);
		}
	}
}