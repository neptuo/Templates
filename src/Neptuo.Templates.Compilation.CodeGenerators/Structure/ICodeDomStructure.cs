using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Describes base structure of generated code.
    /// </summary>
    public interface ICodeDomStructure
    {
        /// <summary>
        /// Naming conventions for this template.
        /// </summary>
        ICodeDomNaming Naming { get; }

        /// <summary>
        /// Whole compilation unit.
        /// </summary>
        CodeCompileUnit Unit { get; }

        /// <summary>
        /// Main generated class.
        /// </summary>
        CodeTypeDeclaration Class { get; }

        /// <summary>
        /// Constructor of main class.
        /// </summary>
        CodeConstructor Constructor { get; }

        /// <summary>
        /// Entry point method/template root.
        /// </summary>
        CodeMemberMethod EntryPoint { get; }
    }
}
