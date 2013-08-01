using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation
{
    public class FileProvider : IFileProvider
    {
        IVirtualPathProvider PathProvider { get; set; }

        public FileProvider(IVirtualPathProvider pathProvider)
        {
            PathProvider = pathProvider;
        }

        public bool Exists(string relativePath)
        {
            return File.Exists(PathProvider.MapPath(relativePath));
        }

        public string GetFileContent(string relativePath)
        {
            return File.ReadAllText(PathProvider.MapPath(relativePath));
        }
    }
}
