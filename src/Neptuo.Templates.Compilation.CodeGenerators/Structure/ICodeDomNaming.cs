using Neptuo.Collections.Specialized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Naming conventions for template code generation.
    /// </summary>
    public interface ICodeDomNaming
    {
        /// <summary>
        /// Collection of custom namig values.
        /// </summary>
        IReadOnlyKeyValueCollection CustomValues { get; }

        /// <summary>
        /// C# namespace for generated class(es).
        /// </summary>
        string NamespaceName { get; }

        /// <summary>
        /// Main class name.
        /// </summary>
        string ClassName { get; }

        /// <summary>
        /// Concatenation of <see cref="ICodeDomNaming.NamespaceName"/> and <see cref="ICodeDomNaming.ClassName"/>.
        /// </summary>
        string FullClassName { get; }

        /// <summary>
        /// Assembly file name.
        /// </summary>
        //string AssemblyFileName { get; }

        /// <summary>
        /// Source code file name.
        /// </summary>
        //string SourceCodeFileName { get; }
    }
}
