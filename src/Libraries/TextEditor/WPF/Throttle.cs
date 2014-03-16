using System.Timers;

namespace TextEditor.WPF
{
    internal class Throttle
    {
        private readonly Timer _timer = new Timer { AutoReset = false };

        public event ElapsedEventHandler Elapsed;

        public Throttle(double interval)
        {
            _timer.Interval = interval;
            _timer.Elapsed += TimerOnElapsed;
        }

        public void Reset()
        {
            _timer.Stop();
            _timer.Start();
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs args)
        {
            if (Elapsed != null)
                Elapsed(sender, args);
        }
    }
}