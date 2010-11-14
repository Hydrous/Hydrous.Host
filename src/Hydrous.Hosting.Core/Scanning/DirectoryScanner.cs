using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydrous.Hosting.Core.Scanning
{
    class DirectoryScanner : IServiceScanner
    {
        public void Scan()
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;

            ScanDirectory(directory);
        }

        private void ScanDirectory(string path)
        {

        }
    }
}
