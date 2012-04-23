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
    using System.IO;
    using System.Threading;

    class Program
    {
        static readonly ILog log = LogManager.GetLogger("Hydrous.Host");

        [LoaderOptimization(LoaderOptimization.MultiDomain)]
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config")));
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

            log.Info("All services shutdown.");
            Environment.ExitCode = 0;
        }

        private static void RunInteractive(string[] args, IServiceController controller)
        {
            var startupArguments = new DefaultStartupArguments();
            var handle = new ManualResetEvent(false);

            Console.CancelKeyPress += (s, e) =>
            {
                if (e.SpecialKey == ConsoleSpecialKey.ControlC)
                {
                    log.Info("Received Ctrl+C command, stopping services.");
                    e.Cancel = true;
                    handle.Set();
                    startupArguments.AbortStartup = true;
                }
            };

            controller.Run(startupArguments);

            ThreadPool.QueueUserWorkItem(o =>
            {
                bool run = true;
                while (run)
                {
                    Console.WriteLine("Type cls to clear screen, or quit|exit to stop.");
                    string input = (Console.ReadLine() ?? "").ToLowerInvariant();

                    switch (input)
                    {
                        case "cls":
                            Console.Clear();
                            break;
                        case "quit":
                        case "exit":
                        {
                            // stop running
                            run = false;
                            log.Info(string.Format("Received {0} command, stopping service.", input));
                            handle.Set();
                            break;
                        }
                        default:
                            Console.WriteLine("'" + input + "' is not a valid operation.");
                            break;
                    }
                }
            });

            handle.WaitOne();

            Console.WriteLine("Shutting down application.");
            var shutdownArguments = new DefaultShutdownArguments();
            controller.Shutdown(shutdownArguments);
        }
    }
}
