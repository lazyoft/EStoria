using System;
using EStoria.Sample.Domain.Aggregates;
using EStoria.Sample.Domain.Commands;
using EStoria.Sample.Domain.Events;
using Machine.Specifications;

namespace EStoria.Sample.Unit.Tests
{
	public class Scheduling_a_transmission
	{
		public class When_scheduling_a_tramsmission_that_doesnt_exist : WithHandler<ScheduleHandler, ScheduleAggregate>
		{
			static DateTime Start = new DateTime(1971, 11, 26, 12, 30, 00);
			static DateTime End = new DateTime(1971, 11, 26, 13, 30, 00);

			It should_raise_a_transmission_scheduled = () => Handler
				.WhenIssuing(new ScheduleTransmission { ChannelId = "01", TransmissionId = "TX01", Start = Start, Duration = TimeSpan.FromHours(1) })
				.ShouldRaise(new TransmissionScheduled { ChannelId = "01", TransmissionId = "TX01", From = Start, To = End });
		}

		public class When_scheduling_a_transmission_that_already_exists : WithHandler<ScheduleHandler, ScheduleAggregate>
		{
			static DateTime Start = new DateTime(1971, 11, 26, 12, 30, 00);
			static DateTime End = new DateTime(1971, 11, 26, 13, 30, 00);

			It should_not_raise_anything_if_nothing_changed = () => Handler
				.Given(new TransmissionScheduled { ChannelId = "01", TransmissionId = "TX01", From = Start, To = End })
				.WhenIssuing(new ScheduleTransmission { ChannelId = "01", TransmissionId = "TX01", Start = Start, Duration = End - Start })
				.ShouldNotRaiseAnything();

			It should_fail_the_command_if_the_transmission_is_canceled = () => Handler
				.Given(
					new TransmissionScheduled { ChannelId = "01", TransmissionId = "TX01", From = Start, To = End },
					new TransmissionCanceled { TransmissionId = "TX01" })
				.WhenIssuing(new ScheduleTransmission { ChannelId = "01", TransmissionId = "TX01", Start = Start.AddHours(1), Duration = TimeSpan.FromHours(1) })
				.ShouldFailTheCommand();

			It should_raise_a_transmission_extended_if_the_duration_has_been_extended = () => Handler
				.Given(new TransmissionScheduled { ChannelId = "01", TransmissionId = "TX01", From = Start, To = End })
				.WhenIssuing(new ScheduleTransmission { ChannelId = "01", TransmissionId = "TX01", Start = Start, Duration = TimeSpan.FromHours(2) })
				.ShouldRaise(new TransmissionExtended { TransmissionId = "TX01", NewDuration = TimeSpan.FromHours(2) });

			It should_raise_a_transmission_shortened_if_the_duration_has_been_shortened = () => Handler
				.Given(new TransmissionScheduled { ChannelId = "01", TransmissionId = "TX01", From = Start, To = End })
				.WhenIssuing(new ScheduleTransmission { ChannelId = "01", TransmissionId = "TX01", Start = Start, Duration = TimeSpan.FromHours(0.5) })
				.ShouldRaise(new TransmissionShortened { TransmissionId = "TX01", Duration = TimeSpan.FromHours(0.5) });

			It should_raise_a_transmission_postponed_if_the_start_has_been_moved_forward_in_time = () => Handler
				.Given(new TransmissionScheduled { ChannelId = "01", TransmissionId = "TX01", From = Start, To = End })
				.WhenIssuing(new ScheduleTransmission { ChannelId = "01", TransmissionId = "TX01", Start = Start.AddHours(1), Duration = TimeSpan.FromHours(1) })
				.ShouldRaise(new TransmissionPostponed { TransmissionId = "TX01", NewStart = Start.AddHours(1) });

			It should_raise_a_transmission_brough_forward_if_the_start_has_been_moved_backward_in_time = () => Handler
				.Given(new TransmissionScheduled { ChannelId = "01", TransmissionId = "TX01", From = Start, To = End })
				.WhenIssuing(new ScheduleTransmission { ChannelId = "01", TransmissionId = "TX01", Start = Start.AddHours(-1), Duration = TimeSpan.FromHours(1) })
				.ShouldRaise(new TransmissionBroughtForward { TransmissionId = "TX01", NewStart = Start.AddHours(-1) });

			It should_raise_a_transmission_moved_if_the_channel_has_been_changed = () => Handler
				.Given(new TransmissionScheduled { ChannelId = "01", TransmissionId = "TX01", From = Start, To = End })
				.WhenIssuing(new ScheduleTransmission { ChannelId = "02", TransmissionId = "TX01", Start = Start, Duration = TimeSpan.FromHours(1) })
				.ShouldRaise(new TransmissionMoved { TransmissionId = "TX01", FromChannelId = "01", ToChannelId = "02" });

			It should_fail_the_command_if_the_change_overlaps_with_another_transmission = () => Handler
				.Given(new TransmissionScheduled { ChannelId = "01", TransmissionId = "TX01", From = Start, To = End })
				.WhenIssuing(new ScheduleTransmission { ChannelId = "01", TransmissionId = "TX02", Start = Start.AddHours(0.5), Duration = TimeSpan.FromHours(1) })
				.ShouldFailTheCommand();
		}
	}
}