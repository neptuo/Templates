using Neptuo.Templates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TestConsoleNG
{
    public class CurrentDirectoryVirtualPathProvider : IVirtualPathProvider
    {
        public string MapPath(string path)
        {
            if (String.IsNullOrEmpty(path))
                return Environment.CurrentDirectory;

            if (path.StartsWith("~/"))
                return Path.Combine(Environment.CurrentDirectory, path.Replace("~/", ""));

            return path;
        }
    }
}
