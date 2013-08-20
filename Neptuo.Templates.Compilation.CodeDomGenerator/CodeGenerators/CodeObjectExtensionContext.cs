using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class CodeObjectExtensionContext
    {
        public CodeDomGenerator.Context CodeDomContext { get; private set; }
        public CodeDomGenerator CodeGenerator { get; private set; }
        public CodeStatementCollection ParentBindStatements { get; private set; }
        public BaseCodeDomStructure BaseStructure { get { return CodeDomContext.BaseStructure; } }
        public string ParentFieldName { get; private set; }

        public CodeObjectExtensionContext(CodeDomGenerator.Context codeDomContext, CodeStatementCollection parentBindStatements, string parentFieldName)
        {
            CodeDomContext = codeDomContext;
            CodeGenerator = codeDomContext.CodeGenerator;
            ParentBindStatements = parentBindStatements;
            ParentFieldName = parentFieldName;
        }
    }
}
