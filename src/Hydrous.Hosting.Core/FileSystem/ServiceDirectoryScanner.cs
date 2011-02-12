using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Hydrous.Hosting.FileSystem
{
    public class ServiceDirectoryScanner : IServiceDirectoryScanner
    {
        private DirectoryInfo ServicesFolder;
        public ServiceDirectoryScanner(DirectoryInfo servicesFolder)
        {
            ServicesFolder = servicesFolder;
        }

        public IEnumerable<ServiceDirectory> Scan()
        {
            foreach (var folder in ServicesFolder.GetDirectories())
            {
                var configFiles = folder.GetFiles(folder.Name + ".config");
                var config = configFiles.FirstOrDefault();

                if (config != null)
                {
                    yield return new ServiceDirectory(folder, config);
                }
            }
        }
    }
}
