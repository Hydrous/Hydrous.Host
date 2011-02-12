﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydrous.Hosting
{
    public interface IServiceController : IDisposable
    {
        void Run();
        void Shutdown();
    }
}
