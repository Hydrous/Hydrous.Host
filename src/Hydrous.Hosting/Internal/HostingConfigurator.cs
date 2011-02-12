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

namespace Hydrous.Hosting.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    class HostingConfigurator<T> : IHostingConfigurator<T>, IBuildServiceWrapper
    {
        private Func<T> Factory { get; set; }
        Action<T> Start { get; set; }
        Action<T> Stop { get; set; }

        public IServiceWrapper Build()
        {
            if (new object[] { Factory, Start, Stop }.Any(x => x == null))
                throw new InvalidOperationException("Required fields have not been set.");

            return new ServiceWrapper<T>(Factory(), Start, Stop);
        }

        public IHostingConfigurator<T> Create(Func<T> factory)
        {
            Factory = factory;
            return this;
        }

        public IHostingConfigurator<T> Named(string name)
        {
            throw new NotImplementedException();
        }

        public IHostingConfigurator<T> DependsOn(WindowsServiceName service)
        {
            throw new NotImplementedException();
        }

        public IHostingConfigurator<T> DependsOn(HostedServiceName service)
        {
            throw new NotImplementedException();
        }

        public IHostingConfigurator<T> WhenStarted(Action<T> callback)
        {
            Start = callback;
            return this;
        }

        public IHostingConfigurator<T> WhenStopped(Action<T> callback)
        {
            Stop = callback;
            return this;
        }

        public IHostingConfigurator<T> WhenPaused(Action<T> callback)
        {
            throw new NotImplementedException();
        }

        public IHostingConfigurator<T> WhenContinued(Action<T> callback)
        {
            throw new NotImplementedException();
        }

        public IHostingConfigurator<T> WhenShutdown(Action<T> callback)
        {
            throw new NotImplementedException();
        }

        public IHostingConfigurator<T> RunAs(Credentials credentials)
        {
            throw new NotImplementedException();
        }
    }
}
