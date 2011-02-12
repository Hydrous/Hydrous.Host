using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Hydrous.Hosting.FileSystem
{
    public class ServiceDirectory
    {
        public ServiceDirectory(DirectoryInfo folder, FileInfo config)
        {
            Folder = folder;
            ConfigurationFile = config;
        }

        public DirectoryInfo Folder { get; private set; }

        public FileInfo ConfigurationFile { get; private set; }
    }
}
