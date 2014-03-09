using System;

namespace EStoria.ValueObjects
{
	public class CommitInfo : IEquatable<CommitInfo>
	{
		public int Serial { get; private set; }
		public string Name { get; private set; }

		public CommitInfo(int serial, string name)
		{
			Serial = serial;
			Name = name;
		}

		public bool Equals(CommitInfo other)
		{
			if(ReferenceEquals(null, other)) return false;
			if(ReferenceEquals(this, other)) return true;
			return Serial == other.Serial && string.Equals(Name, other.Name);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (Serial * 397) ^ Name.GetHashCode();
			}
		}

		public static bool operator ==(CommitInfo left, CommitInfo right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(CommitInfo left, CommitInfo right)
		{
			return !Equals(left, right);
		}

		public static bool operator >(CommitInfo left, CommitInfo right)
		{
			return left.Serial > right.Serial;
		}

		public static bool operator <(CommitInfo left, CommitInfo right)
		{
			return left.Serial < right.Serial;
		}

		public static bool operator >=(CommitInfo left, CommitInfo right)
		{
			return left.Serial >= right.Serial;
		}

		public static bool operator <=(CommitInfo left, CommitInfo right)
		{
			return left.Serial <= right.Serial;
		}
		
		public override bool Equals(object obj)
		{
			if(ReferenceEquals(null, obj)) return false;
			if(ReferenceEquals(this, obj)) return true;
			return obj.GetType() == GetType() && Equals((CommitInfo)obj);
		}
	}
}