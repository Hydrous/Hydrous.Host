using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Hydrous.Hosting
{
    public class HydrousConfiguration :
            ConfigurationSection
    {
        [ConfigurationProperty("Bootstrapper", IsRequired = true)]
        public string Bootstrapper
        {
            get { return (string)this["Bootstrapper"]; }
        }

        public Type BootstrapperType
        {
            get
            {
                string value = Bootstrapper;
                return Type.GetType(value);
            }
        }

        public static HydrousConfiguration GetConfig()
        {
            return ConfigurationManager.GetSection("ServiceConfiguration") as HydrousConfiguration;
        }
    }
}
