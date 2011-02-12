using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydrous.Hosting.FileSystem
{
    public interface IServiceDirectoryScanner
    {
        IEnumerable<ServiceDirectory> Scan();
    }
}
