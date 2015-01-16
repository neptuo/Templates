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
        public XCodeDomGenerator.Context CodeDomContext { get; private set; }

        /// <summary>
        /// Current instance of code dom generator.
        /// </summary>
        public XCodeDomGenerator CodeGenerator { get; private set; }

        /// <summary>
        /// Used base generated class structure.
        /// </summary>
        public CodeDomStructure BaseStructure { get { return CodeDomContext.Structure; } }

        /// <summary>
        /// Custom values (transient) storage.
        /// </summary>
        public Dictionary<string, object> CustomValues { get; private set; }

        /// <summary>
        /// Parent of current field.
        /// </summary>
        public string ParentFieldName { get; private set; }

        public CodeObjectExtensionContext(XCodeDomGenerator.Context codeDomContext, string parentFieldName)
        {
            Guard.NotNull(codeDomContext, "codeDomContext");
            CodeDomContext = codeDomContext;
            CodeGenerator = codeDomContext.CodeGenerator;
            CustomValues = new Dictionary<string, object>();
            ParentFieldName = parentFieldName;
        }
    }
}
