using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydrous.Hosting
{
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
