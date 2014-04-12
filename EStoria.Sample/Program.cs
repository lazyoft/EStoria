using System;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using EStoria.Sample.Domain.Aggregates;
using EStoria.Sample.Domain.Commands;
using EStoria.Sample.Projections;
using EStoria.Sample.Readmodels;
using EStoria.Services;
using EStoria.Services.Persistence.FileSystem;
using EStoria.Services.Serializers;
using EStoria.Services.SerialProviders;
using EStoria.ValueObjects;

namespace EStoria.Sample
{
	class Program
	{
		static void Main(string[] args)
		{
			var bootstrapper = new Bootstrapper();
			bootstrapper.Init();
			bootstrapper.Run();
			foreach(var channel in bootstrapper.ChannelStatistics.Statistics.OrderBy(p => p.Key))
			{
				Console.WriteLine("{0}", channel.Key);
				Console.WriteLine("------------");
				Console.WriteLine("Active....: {0:####0}", channel.Value.Active);
				Console.WriteLine("Canceled..: {0:####0}", channel.Value.Canceled);
				Console.WriteLine("Removed...: {0:####0}", channel.Value.Removed);
				Console.WriteLine("Changes...: {0:####0}", channel.Value.ScheduleChanges);
				Console.WriteLine("------------");
			}
			bootstrapper.Stop();
			Console.ReadLine();
		}
	}

	public class Bootstrapper
	{
		ScheduleHandler _handler;
		SnapshotStore _snapshotStore;
		ChannelStatisticsProjection _channelStatistics;
		ScheduleByDayProjection _scheduleByDay;
		ScheduleByChannelProjection _scheduleByChannel;
		ScheduleAggregate _scheduleAggregate;
		public ScheduleByDay ScheduleByDay { get; private set; }
		public ScheduleByChannel ScheduleByChannel { get; private set; }
		public ChannelStatistics ChannelStatistics { get; private set; }


		public void Init()
		{
			var commitStrategy = new FlatFolderFileCommitStrategy(".json");
			var persistence = new FileCommitPersistence(Path.Combine(Environment.CurrentDirectory, "events"), commitStrategy);
			var serialProvider = new ThreadSafeSerialProvider(persistence.GetLastSerial());
			var clock = new SystemClock();
			var serializer = new JsonSerializer();
			var store = new EventStore(persistence, serialProvider, clock, serializer);
			var snapshotPersistence = new FileCommitPersistence(Path.Combine(Environment.CurrentDirectory, "snapshots"), commitStrategy);
			_snapshotStore = new SnapshotStore(snapshotPersistence, clock, serializer);

			var bus = new MessageBus();
			var eventPublisher = bus.AsPublisher<CommittedEvent>();
			var failurePublisher = bus.AsPublisher<CommandFailure>();
			var domainEventPublisher = bus.AsPublisher<DomainEvent>();

			var loader = new CachedModelLoader(new ModelLoader(store, _snapshotStore, bus.AsObservable<CommittedEvent>()), new InMemoryCache());
			//var loader = new ModelLoader(store, _snapshotStore, bus.AsObservable<CommittedEvent>());
			_handler = new ScheduleHandler(loader);
			_handler.Subscribe<CommandFailure>(failurePublisher.Publish);
			_handler.Subscribe<DomainEvent>(domainEventPublisher.Publish);
			var eventSaver = new DomainEventSaver(_handler, store);
			eventSaver.Subscribe(eventPublisher.Publish);

			_scheduleByDay = loader.Load<ScheduleByDayProjection>("scheduleByDay");
			ScheduleByDay = _scheduleByDay.Model;
			_scheduleByChannel = loader.Load<ScheduleByChannelProjection>("scheduleByChannel");
			ScheduleByChannel = _scheduleByChannel.Model;
			_channelStatistics = loader.Load<ChannelStatisticsProjection>("channelStatistics");
			ChannelStatistics = _channelStatistics.Model;
			_scheduleAggregate = loader.Load<ScheduleAggregate>("channels");

			bus.AsObservable<CommittedEvent>().Subscribe(evt => Console.WriteLine("{0:0000} {1}", evt.Serial, evt.Data));
			bus.AsObservable<CommandFailure>().Subscribe(cf => Console.WriteLine("Command {0} failed because {1}", cf.Command, cf.Reason));
		}

		public void Run()
		{
			var random = new Random();
			for (var i = 0; i < 10000; i++)
			{
				var channel = "C" + random.Next(10);
				var transmission = "TX" + random.Next(100);
				var start = DateTime.Now.AddMinutes(random.Next(1000));
				var duration = TimeSpan.FromMinutes(random.Next(240));
				var type = random.Next(3);
				switch (type)
				{
					case 0:
						_handler.OnNext(new ScheduleTransmission { ChannelId = channel, TransmissionId = transmission, Start = start, Duration = duration });
						break;
					case 1:
						_handler.OnNext(new CancelTransmission { TransmissionId = transmission });
						break;
					case 2:
						_handler.OnNext(new DeleteTransmission { TransmissionId = transmission });
						break;
				}
			}
		}

		public void Stop()
		{
			_snapshotStore.Append("scheduleByDay", _scheduleByDay.Serial, _scheduleByDay.Model);
			_snapshotStore.Append("scheduleByChannel", _scheduleByChannel.Serial, _scheduleByChannel.Model);
			_snapshotStore.Append("channelStatistics", _channelStatistics.Serial, _channelStatistics.Model);
			_snapshotStore.Append("channels", _scheduleAggregate.Serial, _scheduleAggregate.Model);
		}
	}
}
