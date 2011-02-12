// Copyright 2007-2010 The Apache Software Foundation.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.

namespace Hydrous.Host
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.ServiceProcess;
    using log4net;
    using Hydrous.Hosting;

    class Program
    {
        static readonly ILog log = LogManager.GetLogger("Hydrous.Host");

        [LoaderOptimization(LoaderOptimization.MultiDomain)]
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            using (var controller = ControllerFactory.Create())
            {
                if (Environment.UserInteractive)
                {
                    Console.WriteLine("Starting up as console application.");
                    RunInteractive(args, controller);
                }
                else
                {
                    // if we aren't interactive, then we should run as a service
                    ServiceBase.Run(new HydrousService(controller));
                }
            }

            log.Debug("Everything complete. Exiting.");
            System.Threading.Thread.Sleep(3000);
        }

        private static void RunInteractive(string[] args, IServiceController controller)
        {
            controller.Run();
            bool shutdown = false;

            while (!shutdown)
            {
                Console.WriteLine("Type cls to clear screen, or quit|exit to stop.");
                string input = Console.ReadLine() ?? "";

                switch (input.ToLowerInvariant())
                {
                    case "cls":
                        Console.Clear();
                        break;
                    case "quit":
                    case "exit":
                        shutdown = true;
                        break;
                    default:
                        break;
                }
            }

            Console.WriteLine("Shutting down application.");
            controller.Shutdown();
        }
    }
}
