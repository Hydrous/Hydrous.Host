using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hydrous.Hosting;

namespace SimpleClock
{
    public class TimeBroadcasterBootstrapper : IHostedService<TimeBroadcaster>
    {
        public void Initialize(IHostingConfigurator<TimeBroadcaster> config)
        {
            config.Create(() => new TimeBroadcaster())
                .WhenStarted(x => x.Start())
                .WhenStopped(x => x.Stop());
        }
    }
}
