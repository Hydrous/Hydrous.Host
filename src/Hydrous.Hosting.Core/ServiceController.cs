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
    using System.Threading;

    class ServiceController : IServiceController
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ServiceController));

        readonly ICollection<IHostController> Hosts = new HashSet<IHostController>();
        readonly IServiceDirectoryScanner DirectoryScanner;
        readonly IHostControllerFactory HostFactory;
        bool disposed;

        public ServiceController(IServiceDirectoryScanner directoryScanner, IHostControllerFactory hostFactory)
        {
            HostFactory = hostFactory;
            DirectoryScanner = directoryScanner;
        }

        public void Run(IStartupArguments args)
        {
            // create hosts for directories found by scanner
            foreach (var host in DirectoryScanner.Scan().Select(directory => HostFactory.CreateController(this, directory)))
                Hosts.Add(host);

            // start and wait for hosts
            var handles = Hosts.Select(host => StartHost(host, args)).ToArray();
            if (handles.Length != 0)
            {
                if (!WaitHandle.WaitAll(handles, TimeSpan.FromSeconds(120)))
                {
                    log.Warn("Services have taken longer than 2 minutes to start.");
                }
            }

            log.Info("Services running.");
            foreach (var host in Hosts)
                log.Info(string.Format("{0}: {1}", host.Name, host.Status));
        }

        private WaitHandle StartHost(IHostController host, IStartupArguments args)
        {
            var handle = new ManualResetEvent(false);
            new Thread(() =>
            {
                try
                {
                    host.Initialize();
                    host.Start(args);
                }
                catch (Exception ex)
                {
                    log.Error(string.Format("Failed to start '{0}'", host.Name), ex);
                }
                finally
                {
                    handle.Set();
                }
            })
            {
                IsBackground = true
            }.Start();

            return handle;
        }

        public void Shutdown(IShutdownArguments args)
        {
            var handles = Hosts.ToArray().Select(host => ShutdownHost(host, args)).ToArray();
            if (handles.Length > 0)
            {
                if (!WaitHandle.WaitAll(handles, TimeSpan.FromSeconds(100)))
                    log.Error("All services failed to stop in the alotted time.");
            }

            log.Debug("Shutdown.");
        }

        WaitHandle ShutdownHost(IHostController controller, IShutdownArguments args)
        {
            var handle = new ManualResetEvent(false);
            new Thread(() =>
            {
                try
                {
                    using (controller)
                        controller.Stop(args);
                }
                catch (Exception ex)
                {
                    log.Error(string.Format("Failed to shutdown host '{0}'", controller.Name), ex);
                }
                finally
                {
                    handle.Set();
                    Hosts.Remove(controller);
                }
            })
            {
                IsBackground = true
            }
            .Start();

            return handle;
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
    }
}
