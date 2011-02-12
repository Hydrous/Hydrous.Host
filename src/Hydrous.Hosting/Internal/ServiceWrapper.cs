using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace Hydrous.Hosting.Internal
{
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
