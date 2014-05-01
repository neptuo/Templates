using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates
{
    /// <summary>
    /// Enables to map physical file in context of current application path.
    /// </summary>
    public interface IVirtualPathProvider
    {
        /// <summary>
        /// Maps physical file in context of current application path.
        /// </summary>
        /// <param name="path">Application relative file path.</param>
        /// <returns>Absolute file path.</returns>
        string MapPath(string path);
    }
}
