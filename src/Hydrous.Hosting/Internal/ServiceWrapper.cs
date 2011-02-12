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
    using log4net;

    public class ServiceWrapper<T> : IServiceWrapper
    {
        readonly T Service;
        readonly Action<T> StartAction;
        readonly Action<T> StopAction;
        private bool disposed;

        public ServiceWrapper(T service, Action<T> start, Action<T> stop)
        {
            Service = service;
            StartAction = start;
            StopAction = stop;
        }

        public void Start()
        {
            StartAction(Service);
        }

        public void Stop()
        {
            StopAction(Service);
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

            disposed = true;
            if (disposing)
            {
                try
                {
                    var disposable = Service as IDisposable;
                    if (disposable != null)
                        disposable.Dispose();
                }
                catch
                {
                }
            }
        }
    }
}
