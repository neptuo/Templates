using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Describes base generated code structure.
    /// </summary>
    public class CodeDomStructure
    {
        /// <summary>
        /// Compile unit.
        /// </summary>
        public CodeCompileUnit Unit { get; set; }

        /// <summary>
        /// View class.
        /// </summary>
        public CodeTypeDeclaration Class { get; set; }

        /// <summary>
        /// Entry method.
        /// </summary>
        public CodeMemberMethod EntryPointMethod { get; set; }
    }
}
