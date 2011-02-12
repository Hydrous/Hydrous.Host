using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Collections.Concurrent;
using Hydrous.Hosting.FileSystem;
using Hydrous.Hosting.Internal;
using System.Reflection;

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
                CreateAndStartService(serviceDirectory);
            }


            log.Debug("Running.");
        }

        private void CreateAndStartService(ServiceDirectory directory)
        {
            var setup = AppDomain.CurrentDomain.SetupInformation;
            setup.ShadowCopyFiles = "true";
            setup.ConfigurationFile = directory.ConfigurationFile.FullName;
            setup.ApplicationBase = directory.Folder.FullName;

            var domain = AppDomain.CreateDomain("ServiceHost:" + directory.Folder.Name, null, setup);
            try
            {
                ServiceBootstrapper bootstrapper = GetBootstrapper(domain);

                var service = new ServiceHost(directory.Folder.Name, domain, bootstrapper);
                try
                {
                    service.Initialize();
                    service.Start();
                    Services.Push(service);
                }
                catch (Exception ex)
                {
                    log.Error(string.Format("Failed to initialize and start {0} service.", service.Name), ex);
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Failed to create bootstrapper for {0} service.",directory.Folder.Name), ex);
            }
        }

        private static ServiceBootstrapper GetBootstrapper(AppDomain domain)
        {
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
                        service.Stop();
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
