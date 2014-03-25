using System;
using System.Timers;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace DotNetUtils
{
    public class Throttle
    {
        private readonly Timer _timer = new Timer { AutoReset = false };

        public Control Control { get; set; }

        public event ElapsedEventHandler Elapsed;

        public Throttle(double interval)
        {
            _timer.Interval = interval;
            _timer.Elapsed += TimerOnElapsed;
        }

        public Throttle(Control control, double interval)
            : this(interval)
        {
            Control = control;
        }

        public void Reset()
        {
            _timer.Stop();
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs args)
        {
            if (Control != null && Control.InvokeRequired)
            {
                Control.Invoke(new Action(() => TimerOnElapsedImpl(sender, args)));
            }
            else
            {
                TimerOnElapsedImpl(sender, args);
            }
        }

        private void TimerOnElapsedImpl(object sender, ElapsedEventArgs args)
        {
            if (Elapsed != null)
                Elapsed(sender, args);
        }
    }
}