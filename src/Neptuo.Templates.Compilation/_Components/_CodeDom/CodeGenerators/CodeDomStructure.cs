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
        /// Base type of <see cref="CodeDomStructure.Class"/>
        /// </summary>
        public CodeTypeReference BaseType { get { return Class.BaseTypes.OfType<CodeTypeReference>().FirstOrDefault(); } }

        /// <summary>
        /// Entry method.
        /// </summary>
        public CodeMemberMethod EntryPointMethod { get; set; }
    }
}
