using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Configuration
{
    public class ObserverElement : ConfigurationElement
    {
        [ConfigurationProperty("prefix", IsRequired = true)]
        public string Prefix
        {
            get { return (string)this["prefix"]; }
        }

        [ConfigurationProperty("name", IsRequired = false)]
        public string Name
        {
            get { return (string)this["name"]; }
        }

        [ConfigurationProperty("type", IsRequired = true, IsKey = true)]
        public string TypeName
        {
            get { return (string)this["type"]; }
        }

        public Type Type
        {
            get { return Type.GetType(TypeName); }
        }
    }
}
