using System;
using EStoria.ValueObjects;

namespace EStoria.Unit.Tests
{
	public class TestEventModel : EventModel<TestModel>
	{
		public TestEventModel(IObservable<CommittedEvent> events, TestModel modelSnapshot = null, int serial = 0)
			: base(events, modelSnapshot, serial)
		{
			HandleEvents(stream => stream
				.When<string>((model, s) => model.Text += s)
				.When<int>((model, i) => model.Number += i)
				.When<DateTime>((model, time) => model.Date = time)
				.WhenUnknown((model, unknown) => model.Unknown = unknown));
		}
	}
}
