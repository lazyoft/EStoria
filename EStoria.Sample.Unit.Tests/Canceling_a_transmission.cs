using System;
using EStoria.Sample.Domain.Aggregates;
using EStoria.Sample.Domain.Commands;
using EStoria.Sample.Domain.Events;
using Machine.Specifications;

namespace EStoria.Sample.Unit.Tests
{
	public class Canceling_a_transmission
	{
		public class When_canceling_an_existing_transmission : WithHandler<ScheduleHandler, ScheduleAggregate>
		{
			It should_raise_a_transmission_canceled = () => Handler
				.Given(new TransmissionScheduled
				{
					ChannelId = "01", TransmissionId = "TX01", From = new DateTime(1971, 11, 26, 12, 30, 00),
					To = new DateTime(1971, 11, 26, 13, 30, 00)
				})
				.WhenIssuing(new CancelTransmission { TransmissionId = "TX01" })
				.ShouldRaise(new TransmissionCanceled { TransmissionId = "TX01" });
		}

		public class When_canceling_a_transmission_that_doesnt_exist : WithHandler<ScheduleHandler, ScheduleAggregate>
		{
			It should_fail_the_command = () => Handler
				.WhenIssuing(new CancelTransmission { TransmissionId = "TX01" })
				.ShouldFailTheCommand();
		}

		public class When_canceling_an_already_canceled_transmission : WithHandler<ScheduleHandler, ScheduleAggregate>
		{
			It should_fail_the_command = () => Handler
				.Given(
					new TransmissionScheduled { ChannelId = "01", TransmissionId = "TX01" },
					new TransmissionCanceled { TransmissionId = "TX01" })
				.WhenIssuing(new CancelTransmission { TransmissionId = "TX01" })
				.ShouldFailTheCommand();
		}
	}
}