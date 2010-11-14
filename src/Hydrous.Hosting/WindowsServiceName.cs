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

    public class WindowsServiceName
    {
        public WindowsServiceName(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public static readonly WindowsServiceName Msmq = new WindowsServiceName("MSMQ");
        public static readonly WindowsServiceName SqlServer = new WindowsServiceName("MSSQLSERVER");
        public static readonly WindowsServiceName IIS = new WindowsServiceName("W3SVC");
        public static readonly WindowsServiceName EventLog = new WindowsServiceName("EventLog");
    }
}
