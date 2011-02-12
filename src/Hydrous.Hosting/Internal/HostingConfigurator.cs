using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydrous.Hosting.Internal
{
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
