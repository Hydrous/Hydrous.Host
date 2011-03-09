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

namespace Hydrous.Hosting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using log4net;
    using Hydrous.Hosting.FileSystem;
    using Hydrous.Hosting.Internal;
    using System.Reflection;

    public class ServiceController : IServiceController
    {
        static readonly object locker = new object();
        static readonly ILog log = LogManager.GetLogger(typeof(ServiceController));
        private bool disposed;
        readonly Stack<ServiceHost> Services = new Stack<ServiceHost>();
        readonly IServiceDirectoryScanner DirectoryScanner;

        public ServiceController(IServiceDirectoryScanner directoryScanner)
        {
            DirectoryScanner = directoryScanner;
        }

        public void Run(IStartupArguments args)
        {
            lock (locker)
            {
                foreach (var serviceDirectory in DirectoryScanner.Scan())
                {
                    if (args.AbortStartup)
                    {
                        log.Info("Aborting requested, cancelling startup of remaining services.");
                        return;
                    }

                    CreateAndStartService(serviceDirectory);
                }

                log.Debug("Running.");
            }
        }

        private void CreateAndStartService(ServiceDirectory directory)
        {
            var setup = AppDomain.CurrentDomain.SetupInformation;
            setup.ShadowCopyFiles = "true";
            setup.ConfigurationFile = directory.ConfigurationFile.FullName;
            setup.ApplicationBase = directory.Folder.FullName;


            var domainName = "ServiceHost." + directory.Folder.Name;
            log.Info(string.Format("Creating {0} app domain", domainName));
            var domain = AppDomain.CreateDomain(domainName, null, setup);
            try
            {
                ServiceBootstrapper bootstrapper = GetBootstrapper(domain);

                var service = new ServiceHost(directory.Folder.Name, domain, bootstrapper);
                try
                {
                    log.Info(string.Format("[{0}] Initializing service.", domainName));
                    service.Initialize();
                    log.Info(string.Format("[{0}] Starting service.", domainName));
                    service.Start();
                    log.Debug(string.Format("[{0}] Registering service.", domainName));
                    Services.Push(service);
                }
                catch (Exception ex)
                {
                    log.Error(string.Format("[{0}] Failed to initialize and start service.", service.Name), ex);
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("[{0}] Failed to create bootstrapper for service.", directory.Folder.Name), ex);
            }
        }

        private static ServiceBootstrapper GetBootstrapper(AppDomain domain)
        {
            log.Debug(string.Format("[{0}] Creating bootstrapper.", domain.FriendlyName));
            var bootstrapperType = typeof(ServiceBootstrapper);
            var bootstrapper = (ServiceBootstrapper)domain.CreateInstanceAndUnwrap(
                bootstrapperType.Assembly.FullName,
                bootstrapperType.FullName,
                true,
                BindingFlags.Default,
                null,
                null,
                System.Globalization.CultureInfo.CurrentCulture,
                null,
                null
            );

            return bootstrapper;
        }

        public void Shutdown(IShutdownArguments args)
        {
            lock (locker)
            {
                // stop each service that's running
                // dispose each service
                while (Services.Count > 0)
                {
                    string serviceName = null;
                    try
                    {
                        using (var service = Services.Pop())
                        {
                            serviceName = service.Name;
                            try
                            {
                                log.Info(string.Format("[{0}] Stopping service.", serviceName));
                                service.Stop();
                            }
                            catch (Exception ex)
                            {
                                log.Error(string.Format("[{0}] Failed to stop service.", serviceName), ex);
                            }
                            finally
                            {
                                log.Info(string.Format("[{0}] Disposing service.", serviceName));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(string.Format("[{0}] Failed to dispose service.", serviceName ?? "(unknown)"), ex);
                    }
                }
            }

            log.Debug("Shutdown.");
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
                log.Debug("Disposed.");
            }
        }

        private class ServiceHost : IDisposable
        {
            bool disposed;
            static readonly ILog log = LogManager.GetLogger(typeof(ServiceHost));
            public readonly string Name;
            readonly AppDomain Domain;
            readonly ServiceBootstrapper Bootstrapper;

            public ServiceHost(string name, AppDomain domain, ServiceBootstrapper bootstrapper)
            {
                Name = name;
                Domain = domain;
                Bootstrapper = bootstrapper;
            }

            public void Initialize()
            {
                Bootstrapper.Initialize();
            }

            public void Start()
            {
                Bootstrapper.Start();
            }

            public void Stop()
            {
                Bootstrapper.Stop();
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
                    Bootstrapper.Dispose();
                    log.Debug("Disposed.");
                }
            }
        }
    }
}
