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

    public class Credentials
    {
        public Credentials(string domain, string username, string password)
        {
            DomainName = domain;
            Username = username;
            Password = password;
        }

        public string DomainName { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }

        public static readonly Credentials NetworkService = new Credentials("NT AUTHORITY", "NETWORK SERVICE", "");
        public static readonly Credentials LocalSystem = new Credentials("NT AUTHORITY", "SYSTEM", "");
        public static readonly Credentials LocalService = new Credentials("NT AUTHORITY", "LOCAL SERVICE", "");
    }
}
