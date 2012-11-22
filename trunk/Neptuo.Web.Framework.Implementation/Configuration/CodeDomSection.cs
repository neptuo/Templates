using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Configuration
{
    public class CodeDomSection : ConfigurationSection
    {
        [ConfigurationProperty("tempDirectory")]
        public string TempDirectory
        {
            get { return (string)this["tempDirectory"]; }
        }

        [ConfigurationProperty("debugMode")]
        public bool DebugMode
        {
            get { return (bool)this["debugMode"]; }
        }

        [ConfigurationProperty("bin", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(PathCollection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public PathCollection BinDirectories
        {
            get { return (PathCollection)this["bin"]; }
        }
    }
}
