﻿// Copyright 2007-2010 The Apache Software Foundation.
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

    public interface IStartupArguments
    {
        /// <summary>
        /// Gets or sets a value indicating whether the startup should be aborted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if startup should be aborted; otherwise, <c>false</c>.
        /// </value>
        bool AbortStartup { get; set; }
    }
}
