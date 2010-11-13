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

    public interface IHostingConfigurator<T>
    {
        /// <summary>
        /// Specifies the name that identifies the service
        /// </summary>
        /// <param name="name">The name of the service</param>
        IHostingConfigurator<T> Named(string name);

        /// <summary>
        /// Specifies that the hosted service depends on a windows service.
        /// </summary>
        /// <param name="service">The windows service name.</param>
        IHostingConfigurator<T> DependsOn(WindowsServiceName service);

        /// <summary>
        /// Specifies that the hosted service depends on another hosted service.
        /// </summary>
        /// <param name="service">The name of another service being hosted in the current process.</param>
        IHostingConfigurator<T> DependsOn(HostedServiceName service);

        /// <summary>
        /// Specifies the action to execute when the request to start the service is received.
        /// </summary>
        /// <param name="callback">The action to execute.</param>
        IHostingConfigurator<T> WhenStarted(Action<T> callback);

        /// <summary>
        /// Specifies the action to execute when the request to stop the service is received.
        /// </summary>
        /// <param name="callback">The action to execute.</param>
        IHostingConfigurator<T> WhenStopped(Action<T> callback);

        /// <summary>
        /// Specifies the action to execute when the request to pause the service is received.
        /// </summary>
        /// <param name="callback">The action to execute.</param>
        IHostingConfigurator<T> WhenPaused(Action<T> callback);

        /// <summary>
        /// Specifies the action to execute when the request to continue the service is received.
        /// </summary>
        /// <param name="callback">The action to execute.</param>
        IHostingConfigurator<T> WhenContinued(Action<T> callback);

        /// <summary>
        /// Specifies the action to execute when the request to shutdown the service is received.
        /// </summary>
        /// <param name="callback">The action to execute.</param>
        /// <remarks>This is called only when the system is shutting down. 
        /// By default if no action is provided the shutdown action is invoked.
        /// </remarks>
        IHostingConfigurator<T> WhenShutdown(Action<T> callback);
    }
}
