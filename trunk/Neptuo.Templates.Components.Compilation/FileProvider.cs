using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation
{
    /// <summary>
    /// Standart file system implementation of <see cref="IFileProvider"/>.
    /// </summary>
    public class FileProvider : IFileProvider
    {
        /// <summary>
        /// Virtual path resolver.
        /// </summary>
        public IVirtualPathProvider PathProvider { get; private set; }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="pathProvider">Virtual path resolver.</param>
        public FileProvider(IVirtualPathProvider pathProvider)
        {
            Guard.NotNull(pathProvider, "pathProvider");
            PathProvider = pathProvider;
        }

        /// <summary>
        /// Returns true is <paramref name="relativePath"/> exists in file system.
        /// </summary>
        /// <param name="relativePath">Relative file path.</param>
        /// <returns>True is <paramref name="relativePath"/> exists in file system.</returns>
        public bool Exists(string relativePath)
        {
            Guard.NotNull(relativePath, "relativePath");
            return File.Exists(PathProvider.MapPath(relativePath));
        }

        /// <summary>
        /// Returns content of file at <paramref name="relativePath"/>.
        /// </summary>
        /// <param name="relativePath">Relative file path.</param>
        /// <returns>Content of file at <paramref name="relativePath"/>.</returns>
        public string GetFileContent(string relativePath)
        {
            Guard.NotNull(relativePath, "relativePath");
            return File.ReadAllText(PathProvider.MapPath(relativePath));
        }
    }
}
