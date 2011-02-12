using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydrous.Hosting
{
    public static class ControllerFactory
    {
        public static IServiceController Create()
        {
            return new ServiceController();
        }
    }
}
