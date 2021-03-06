﻿// Copyright 2007-2010 The Apache Software Foundation.
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
    using Hydrous.Hosting;
    using System.IO;

    public class TimeBroadcasterBootstrapper : IHostedService<TimeBroadcaster>
    {
        public TimeBroadcasterBootstrapper()
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config")));
        }

        public void Initialize(IHostingConfigurator<TimeBroadcaster> config)
        {
            config.Create(() => new TimeBroadcaster())
                .WhenStarted(x => x.Start())
                .WhenStopped(x => x.Stop());
        }
    }
}
