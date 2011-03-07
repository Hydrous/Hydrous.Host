using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydrous.Hosting
{
    public class DefaultStartupArguments : IStartupArguments
    {
        public bool AbortStartup { get; set; }
    }
}
