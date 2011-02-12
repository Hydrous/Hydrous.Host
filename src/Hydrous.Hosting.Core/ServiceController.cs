using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Collections.Concurrent;
using Hydrous.Hosting.FileSystem;

namespace Hydrous.Hosting
{
    public class ServiceController : IServiceController
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ServiceController));
        private bool disposed;
        readonly ConcurrentStack<ServiceHost> Services = new ConcurrentStack<ServiceHost>();
        readonly IServiceDirectoryScanner DirectoryScanner;

        public ServiceController(IServiceDirectoryScanner directoryScanner)
        {
            DirectoryScanner = directoryScanner;
        }

        public void Run()
        {
            foreach (var serviceDirectory in DirectoryScanner.Scan())
            {

            }
            // scan for service directories
            // create host for each service
            // start service
            log.Debug("Running.");
        }

        public void Shutdown()
        {
            // stop each service that's running
            // dispose each service
            while (Services.Count > 0)
            {
                ServiceHost service;
                if (Services.TryPop(out service))
                {
                    using (service)
                    {
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
            public ServiceHost(string directory)
            {

            }

            public void Start()
            {

            }

            public void Stop()
            {
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
}
