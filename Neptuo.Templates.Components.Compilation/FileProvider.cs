using Neptuo.FileSystems;
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
        private readonly IReadOnlyDirectory directory;

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="directory">Root directory to search in.</param>
        public FileProvider(IReadOnlyDirectory directory)
        {
            Guard.NotNull(directory, "directory");
            this.directory = directory;
        }

        /// <summary>
        /// Returns true is <paramref name="relativePath"/> exists in file system.
        /// </summary>
        /// <param name="relativePath">Relative file path.</param>
        /// <returns>True is <paramref name="relativePath"/> exists in file system.</returns>
        public bool Exists(string relativePath)
        {
            Guard.NotNull(relativePath, "relativePath");
            return directory.FindFiles(relativePath, true).Any();
        }

        /// <summary>
        /// Returns content of file at <paramref name="relativePath"/>.
        /// </summary>
        /// <param name="relativePath">Relative file path.</param>
        /// <returns>Content of file at <paramref name="relativePath"/>.</returns>
        public string GetFileContent(string relativePath)
        {
            Guard.NotNull(relativePath, "relativePath");
            if (!Exists(relativePath))
                return null;

            return directory.FindFiles(relativePath, true).First().GetContentAsync().Result;
        }
    }
}
