using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation
{
    /// <summary>
    /// Defines naming structure for compiled view.
    /// </summary>
    public interface INaming
    {
        /// <summary>
        /// View name.
        /// </summary>
        string SourceName { get; }

        /// <summary>
        /// Namespace to generated class to.
        /// </summary>
        string ClassNamespace { get; }

        /// <summary>
        /// Name of generated class.
        /// </summary>
        string ClassName { get; }

        /// <summary>
        /// Name of assembly to compile class to.
        /// </summary>
        string AssemblyName { get; }
    }
}
