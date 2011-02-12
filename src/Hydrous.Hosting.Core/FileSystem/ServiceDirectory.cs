using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydrous.Hosting.FileSystem
{
    public class ServiceDirectory
    {
        public ServiceDirectory(string path, string config)
        {
            Path = path;
            ConfigurationFile = config;
        }

        public string Path { get; private set; }

        public string ConfigurationFile { get; private set; }
    }
}
