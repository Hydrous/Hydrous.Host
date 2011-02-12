using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Reflection;

namespace Hydrous.Hosting.Internal
{
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
