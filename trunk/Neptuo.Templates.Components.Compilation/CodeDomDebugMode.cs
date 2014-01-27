using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation
{
    /// <summary>
    /// Debug options for <see cref="CodeDomViewService"/>.
    /// </summary>
    [Flags]
    public enum CodeDomDebugMode
    {
        /// <summary>
        /// Generates C# source files named after class name.
        /// </summary>
        GenerateSourceCode,

        /// <summary>
        /// Generates PDB debuging files.
        /// </summary>
        GeneratePdb,

        /// <summary>
        /// Ignores whether assembly files exists and always recompile it.
        /// </summary>
        AlwaysReGenerate
    }
}
