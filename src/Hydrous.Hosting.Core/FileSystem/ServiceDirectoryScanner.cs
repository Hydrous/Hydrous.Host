using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydrous.Hosting.FileSystem
{
    public class ServiceDirectoryScanner : IServiceDirectoryScanner
    {
        public IEnumerable<ServiceDirectory> Scan()
        {
            yield break;
        }
    }
}
