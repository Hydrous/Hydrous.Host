using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hydrous.Hosting.FileSystem;

namespace Hydrous.Hosting
{
    interface IHostControllerFactory
    {
        IHostController CreateController(IServiceController controller, ServiceDirectory directory);
    }

    class HostControllerFactory : IHostControllerFactory
    {
        public IHostController CreateController(IServiceController controller, ServiceDirectory directory)
        {
            return new HostController(controller, directory);
        }
    }
}
