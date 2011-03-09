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

namespace Hydrous.Host
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.ServiceProcess;
    using log4net;
    using Hydrous.Hosting;

    public class HydrousService : ServiceBase
    {
        static readonly ILog log = LogManager.GetLogger(typeof(HydrousService));
        readonly IServiceController Controller;
        readonly IStartupArguments StartupArgs = new DefaultStartupArguments();
        readonly IShutdownArguments ShutdownArgs = new DefaultShutdownArguments();

        public HydrousService(IServiceController controller)
        {
            Controller = controller;
        }

        protected override void OnStart(string[] args)
        {
            log.Debug("Received start command from windows service host.");
            this.RequestAdditionalTime((int)TimeSpan.FromSeconds(60).TotalMilliseconds);
            Controller.Run(StartupArgs);
        }

        protected override void OnStop()
        {
            log.Debug("Received stop command from windows service host.");
            this.RequestAdditionalTime((int)TimeSpan.FromSeconds(60).TotalMilliseconds);
            StartupArgs.AbortStartup = true;
            Controller.Shutdown(ShutdownArgs);
        }

        protected override void OnShutdown()
        {
            log.Debug("Received shutdown command from windows service host.");
            StartupArgs.AbortStartup = true;
            Controller.Shutdown(ShutdownArgs);
        }
    }
}
