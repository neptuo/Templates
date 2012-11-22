using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Configuration
{
    public class PathElement : ConfigurationElement
    {
        /// <summary>
        /// Dot (.) instead of current executing directory.
        /// </summary>
        [ConfigurationProperty("path", IsRequired = true, IsKey = true)]
        public string Path
        {
            get
            {
                string path = (string)this["path"];
                if (path == ".")
                    return Environment.CurrentDirectory;

                return path;
            }
        }
    }
}
