using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydrous.Hosting
{
    interface IHostController : IDisposable
    {
        string Name { get; }

        HostStatus Status { get; }

        void Initialize();

        void Start(IStartupArguments arguments);

        void Stop(IShutdownArguments arguments);
    }
}
