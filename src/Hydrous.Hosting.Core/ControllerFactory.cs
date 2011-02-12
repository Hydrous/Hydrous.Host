using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hydrous.Hosting.FileSystem;
using System.IO;

namespace Hydrous.Hosting
{
    public static class ControllerFactory
    {
        public static IServiceController Create()
        {
            var currentDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            var servicesDirectory = currentDirectory.GetDirectories().First(x => string.Equals(x.Name, "Services", StringComparison.CurrentCultureIgnoreCase));

            return new ServiceController(new ServiceDirectoryScanner(servicesDirectory));
        }
    }
}
