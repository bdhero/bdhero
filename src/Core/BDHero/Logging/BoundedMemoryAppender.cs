using System.Collections.Concurrent;
using System.Linq;
using DotNetUtils.Annotations;
using DotNetUtils.Extensions;
using log4net;
using log4net.Appender;
using log4net.Core;

namespace BDHero.Logging
{
    [UsedImplicitly]
    internal class BoundedMemoryAppender : MemoryAppender
    {
        private readonly ConcurrentQueue<FormattedLoggingEvent> _events = new ConcurrentQueue<FormattedLoggingEvent>();

        private const int MaxEvents = 50;

        private FormattedLoggingEvent[] RecentEventsInternal
        {
            get { return _events.ToArray(); }
        }

        protected override void Append(LoggingEvent @event)
        {
            _events.Enqueue(new FormattedLoggingEvent(@event, Layout));

            while (_events.Count > MaxEvents)
            {
                FormattedLoggingEvent dummy;
                _events.TryDequeue(out dummy);
            }
        }

        protected override void Append(LoggingEvent[] loggingEvents)
        {
        }

        public static FormattedLoggingEvent[] RecentEvents
        {
            get
            {
                var memoryAppender = LogManager.GetRepository().GetAppenders().OfType<BoundedMemoryAppender>().FirstOrDefault();
                return memoryAppender == null ? new FormattedLoggingEvent[0] : Mutate(memoryAppender.RecentEventsInternal);
            }
        }

        private static FormattedLoggingEvent[] Mutate(FormattedLoggingEvent[] events)
        {
            events.ForEach(Mutate);
            return events;
        }

        private static void Mutate(FormattedLoggingEvent @event, int i, int len)
        {
            @event.IsLast = i == len - 1;
        }
    }
}