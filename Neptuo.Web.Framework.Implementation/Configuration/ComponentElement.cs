using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Configuration
{
    public class ComponentElement : ConfigurationElement
    {
        [ConfigurationProperty("prefix", IsRequired = false)]
        public string Prefix
        {
            get { return (string)this["prefix"]; }
        }

        [ConfigurationProperty("namespace", IsRequired = true, IsKey = true)]
        public string Namespace
        {
            get { return (string)this["namespace"]; }
        }
    }
}
