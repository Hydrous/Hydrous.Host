// Copyright 2007-2010 The Apache Software Foundation.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.

namespace SimpleClock
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Timers;
    using log4net;

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
