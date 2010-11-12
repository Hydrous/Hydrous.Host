using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;

namespace Hydrous.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                RunInteractive(args);
            }
            else
            {
                // if we aren't interactive, then we should run as a service
                ServiceBase.Run(new HydrousService());
            }
        }

        private static void RunInteractive(string[] args)
        {
            // TODO: run with interactive console
            // TODO: allow commands like stop [service], restart, start, etc
            throw new NotImplementedException();
        }
    }
}
