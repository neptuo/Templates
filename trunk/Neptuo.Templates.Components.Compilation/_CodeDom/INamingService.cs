using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation
{
    /// <summary>
    /// Service used for naming generated views.
    /// </summary>
    public interface INamingService
    {
        /// <summary>
        /// Creates naming for file.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <returns>Naming for file.</returns>
        INaming FromFile(string fileName);

        /// <summary>
        /// Creates naming for content of template.
        /// </summary>
        /// <param name="viewContent">Template content.</param>
        /// <returns>Naming for content of template.</returns>
        INaming FromContent(string viewContent);
    }
}