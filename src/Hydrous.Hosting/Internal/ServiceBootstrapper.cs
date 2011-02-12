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
    using System.Reflection;

    public class ServiceBootstrapper : MarshalByRefObject, IDisposable
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ServiceBootstrapper));
        private bool disposed;
        IServiceWrapper Wrapper { get; set; }

        public ServiceBootstrapper()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public void Initialize()
        {
            // load type from configuration
            var bootstrapperType = HydrousConfiguration.GetConfig().BootstrapperType;
            var serviceType = bootstrapperType.GetInterface(typeof(IHostedService<>).Name).GetGenericArguments()[0];

            try
            {
                Wrapper = typeof(ServiceBootstrapper).GetMethod("CreateWrapper", BindingFlags.Default | BindingFlags.Static | BindingFlags.NonPublic)
                    .MakeGenericMethod(bootstrapperType, serviceType)
                    .Invoke(null, new object[0]) as IServiceWrapper;
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }

        private static IServiceWrapper CreateWrapper<B,S>()
            where B : class, IHostedService<S>, new()
            where S : class
        {
            var hoster = new B();
            var configurator = new HostingConfigurator<S>();
            hoster.Initialize(configurator);

            return configurator.Build();
        }

        public void Start()
        {
            // call start method
            Wrapper.Start();
        }

        public void Stop()
        {
            Wrapper.Stop();
        }

        public override object InitializeLifetimeService()
        {
            return null;
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
                Wrapper.Dispose();
                log.Debug("Disposed.");
            }
        }
    }
}
