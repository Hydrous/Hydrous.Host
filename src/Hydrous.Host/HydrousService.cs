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

    public class HydrousService : ServiceBase
    {
        static readonly ILog log = LogManager.GetLogger(typeof(HydrousService));
        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
        }

        protected override void OnStop()
        {
            base.OnStop();
        }
        protected override void OnShutdown()
        {
            base.OnShutdown();
        }
    }
}
