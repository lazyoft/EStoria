using System;

namespace EStoria.ValueObjects
{
	public class CommittedEvent : Commit {
		public CommittedEvent(int serial, string name, DateTime timeStamp, object data) : base(serial, name, timeStamp, data) {}
	}
}