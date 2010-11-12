using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;

namespace Hydrous.Host
{
    public class HydrousService : ServiceBase
    {
        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
        }

        protected override void OnStop()
        {
            base.OnStop();
        }
        protected override void OnShutdown()
        {
            base.OnShutdown();
        }
    }
}
