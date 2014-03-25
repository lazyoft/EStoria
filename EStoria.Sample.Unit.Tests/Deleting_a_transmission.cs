using System;
using EStoria.Sample.Domain.Aggregates;
using EStoria.Sample.Domain.Commands;
using EStoria.Sample.Domain.Events;
using Machine.Specifications;

namespace EStoria.Sample.Unit.Tests
{
	public class Deleting_a_transmission
	{
		public class When_deleting_an_existing_transmission : WithHandler<ScheduleHandler, ScheduleAggregate>
		{
			It should_raise_a_transmission_deleted = () => Handler
				.Given(new TransmissionScheduled { TransmissionId = "TX01", ChannelId = "01" })
				.WhenIssuing(new DeleteTransmission { TransmissionId = "TX01" })
				.ShouldRaise(new TransmissionDeleted { TransmissionId = "TX01" });
		}

		public class When_deleting_a_transmission_that_doesnt_exist : WithHandler<ScheduleHandler, ScheduleAggregate>
		{
			It should_fail_the_command = () => Handler
				.WhenIssuing(new DeleteTransmission { TransmissionId = "TX01" })
				.ShouldFailTheCommand();
		}
	}
}
