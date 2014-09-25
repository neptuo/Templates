using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Describes context of sub generator execution.
    /// </summary>
    public class CodeObjectExtensionContext
    {
        /// <summary>
        /// Inner code dom generator context.
        /// </summary>
        public CodeDomGenerator.Context CodeDomContext { get; private set; }

        /// <summary>
        /// Current instance of code dom generator.
        /// </summary>
        public CodeDomGenerator CodeGenerator { get; private set; }

        /// <summary>
        /// Used base generated class structure.
        /// </summary>
        public CodeDomStructure BaseStructure { get { return CodeDomContext.Structure; } }

        /// <summary>
        /// Parent of current field.
        /// </summary>
        public string ParentFieldName { get; private set; }

        public CodeObjectExtensionContext(CodeDomGenerator.Context codeDomContext, string parentFieldName)
        {
            Guard.NotNull(codeDomContext, "codeDomContext");
            CodeDomContext = codeDomContext;
            CodeGenerator = codeDomContext.CodeGenerator;
            ParentFieldName = parentFieldName;
        }
    }
}
