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

namespace Hydrous.Hosting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Hydrous.Hosting.FileSystem;
    using log4net;
    using Hydrous.Hosting.Internal;

    class HostController : IHostController
    {
        static readonly ILog log = LogManager.GetLogger(typeof(HostController));

        readonly object locker = new object();
        readonly IServiceController _Controller;
        readonly AppDomain Domain;

        public HostController(IServiceController controller, ServiceDirectory directory)
        {
            _Controller = controller;
            Name = directory.Folder.Name;
            Status = HostStatus.Created;

            // create our app domain
            var setup = AppDomain.CurrentDomain.SetupInformation;
            setup.ShadowCopyFiles = "true";
            setup.ConfigurationFile = directory.ConfigurationFile.FullName;
            setup.ApplicationBase = directory.Folder.FullName;

            log.Debug(string.Format("[{0}] Creating AppDomain.", Name));
            Domain = AppDomain.CreateDomain("ServiceHost." + Name, null, setup);
        }

        private ServiceBootstrapper Bootstrapper { get; set; }

        public HostStatus Status { get; private set; }

        public string Name { get; private set; }

        public void Initialize()
        {
            lock (locker)
            {
                if (Status == HostStatus.Created)
                {
                    WriteLog("Creating bootstrapper.", log.Debug);

                    var bootstrapperType = typeof(ServiceBootstrapper);
                    Bootstrapper = (ServiceBootstrapper)Domain.CreateInstanceAndUnwrap(
                        assemblyName: bootstrapperType.Assembly.FullName,
                        typeName: bootstrapperType.FullName,
                        ignoreCase: true,
                        bindingAttr: System.Reflection.BindingFlags.Default,
                        binder: null,
                        args: null,
                        culture: System.Globalization.CultureInfo.CurrentCulture,
                        activationAttributes: null,
                        securityAttributes: null
                    );

                    WriteLog("Initializing service.");
                    Bootstrapper.Initialize();
                    Status = HostStatus.Initialized;
                }
            }
        }

        public void Start(IStartupArguments arguments)
        {
            lock (locker)
            {
                if (Status == HostStatus.Initialized || Status == HostStatus.Stopped)
                {
                    WriteLog("Starting service.");
                    Status = HostStatus.Starting;
                    Bootstrapper.Start();

                    Status = HostStatus.Running;
                    WriteLog("Service running.");
                }
            }
        }

        public void Stop(IShutdownArguments arguments)
        {
            lock (locker)
            {
                try
                {
                    if (Status == HostStatus.Running)
                    {
                        WriteLog("Stopping service.", log.Debug);
                        Status = HostStatus.Stopping;
                        Bootstrapper.Stop();
                    }
                }
                finally
                {
                    Status = HostStatus.Stopped;
                    WriteLog("Service stopped.");
                }
            }
        }

        void WriteLog(string message, Action<object> callback = null)
        {
            callback = callback ?? log.Info;

            callback(string.Format("{0} ({1}) - {2}", Name, Status, message));
        }

        public void Dispose()
        {
            try
            {
                if (Bootstrapper != null)
                {
                    WriteLog("Service disposing.", log.Debug);
                    Bootstrapper.Dispose();
                }
            }
            finally
            {
                WriteLog("Unloading AppDomain.", log.Debug);
                AppDomain.Unload(Domain);
            }
        }
    }
}
