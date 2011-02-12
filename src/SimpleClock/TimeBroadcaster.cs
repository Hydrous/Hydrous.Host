using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using log4net;

namespace SimpleClock
{
    public class TimeBroadcaster : IDisposable
    {
        static readonly ILog log = LogManager.GetLogger(typeof(TimeBroadcaster));
        Timer Timer;
        private bool disposed;
        public TimeBroadcaster()
        {
            Timer = new Timer(1000);
            Timer.Elapsed += new ElapsedEventHandler(OnElapsed);
        }

        void OnElapsed(object sender, ElapsedEventArgs e)
        {
            log.Info("The time is " + e.SignalTime.TimeOfDay.ToString());
        }

        public void Start()
        {
            log.Info("Starting broadcasting...");
            Timer.Start();
        }

        public void Stop()
        {
            log.Info("Stopping broadcasting...");
            Timer.Stop();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                log.Info("Disposing of broadcaster.");
                Timer.Dispose();
            }

            disposed = true;
        }
    }
}
