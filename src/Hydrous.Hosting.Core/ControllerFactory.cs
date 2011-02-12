using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hydrous.Hosting.FileSystem;

namespace Hydrous.Hosting
{
    public static class ControllerFactory
    {
        public static IServiceController Create()
        {
            return new ServiceController(new ServiceDirectoryScanner());
        }
    }
}
