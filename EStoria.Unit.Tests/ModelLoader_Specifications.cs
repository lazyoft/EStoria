using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using EStoria.Interfaces;
using EStoria.ValueObjects;
using FluentAssertions;
using Machine.Fakes;
using Machine.Specifications;

namespace EStoria.Unit.Tests
{
	public class ModelLoaderSetup : WithSubject<ModelLoader>
	{
		protected static CommittedSnapshot Snapshot;
		protected static TestEventModel EventModel;
		protected static Subject<CommittedEvent> Events;
		protected static IList<CommittedEvent> AdditionalEvents;

		Establish context = () =>
		{
			Events = new Subject<CommittedEvent>();
			AdditionalEvents = new[] { new CommittedEvent(43, "test", DateTime.Now, " with some data"), new CommittedEvent(44, "test", DateTime.Now, 10) };
			Snapshot = new CommittedSnapshot(42, "test", DateTime.Now, new TestModel { Text = "test" });
			Configure(registrar => registrar.For<IObservable<CommittedEvent>>().Use(Events));
			The<ISnapshotStore>().WhenToldTo(store => store.Read("test", Int32.MaxValue)).Return(Snapshot);
		};

	}

	[Subject(typeof(ModelLoader))]
	public class When_told_to_load_a_model : ModelLoaderSetup
	{
		Because of = () => EventModel = Subject.Load<TestEventModel>("test");

		It should_read_the_latest_snapshot = () => The<ISnapshotStore>().WasToldTo(store => store.Read("test", Int32.MaxValue)).OnlyOnce();
		It should_read_the_events_from_the_store_for_that_id_starting_from_the_snapshot_serial = () => The<IEventStore>().WasToldTo(store => store.Read("test", 42)).OnlyOnce();
		It should_return_the_model = () => EventModel.Should().NotBeNull();
	}

	[Subject(typeof(ModelLoader))]
	public class When_the_model_read_from_the_snapshot_is_not_up_to_date : ModelLoaderSetup
	{
		Establish context = () => The<IEventStore>().WhenToldTo(store => store.Read("test", 42)).Return(AdditionalEvents);
		Because of = () => EventModel = Subject.Load<TestEventModel>("test");
		It should_apply_the_remaining_events_to_the_snapshot = () => EventModel.Model.Text.Should().Be("test with some data");
	}

	[Subject(typeof(ModelLoader))]
	public class When_creating_a_model : ModelLoaderSetup
	{
		Because of = () =>
		{
			EventModel = Subject.Load<TestEventModel>("test");
			Events.OnNext(new CommittedEvent(43, "test", DateTime.Now, 1000));
		};

		It should_assign_the_Observable_of_CommittedEvents_to_the_model = () => EventModel.Model.Number.Should().Be(1000);
	}
}