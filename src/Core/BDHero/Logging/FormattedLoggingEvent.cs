using System.IO;
using log4net.Core;
using log4net.Layout;

namespace BDHero.Logging
{
    internal class FormattedLoggingEvent
    {
        private readonly LoggingEvent _event;
        private readonly ILayout _layout;

        public bool IsLast;

        public FormattedLoggingEvent(LoggingEvent @event, ILayout layout)
        {
            _event = @event;
            _layout = layout;
        }

        public override string ToString()
        {
            return ToString(false);
        }

        public string ToString(bool excludeLastExceptionStackTrace)
        {
            using (var writer = new StringWriter())
            {
                _layout.Format(writer, _event);

                var line = writer.ToString();
                var exception = _event.GetExceptionString();

                var exclude = (IsLast && excludeLastExceptionStackTrace);
                return string.IsNullOrEmpty(exception) || exclude ? line : string.Format("{0}\n{1}", line, exception);
            }
        }
    }
}