using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation
{
    /// <summary>
    /// Abstration for file system access.
    /// </summary>
    public interface IFileProvider
    {
        /// <summary>
        /// Returns true is <paramref name="relativePath"/> exists in file system.
        /// </summary>
        /// <param name="relativePath">Relative file path.</param>
        /// <returns>True is <paramref name="relativePath"/> exists in file system.</returns>
        bool Exists(string relativePath);

        /// <summary>
        /// Returns content of file at <paramref name="relativePath"/>.
        /// </summary>
        /// <param name="relativePath">Relative file path.</param>
        /// <returns>Content of file at <paramref name="relativePath"/>.</returns>
        string GetFileContent(string relativePath);
    }
}
