using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.CodeGenerators.Extensions
{
    public class CodeDomCodeObjectExtensionContext
    {
        public CodeDomGenerator CodeGenerator { get; private set; }
        public CodeMemberMethod ParentBindMethod { get; private set; }
        public string ParentFieldName { get; private set; }

        public CodeDomCodeObjectExtensionContext(CodeDomGenerator codeGenerator, CodeMemberMethod parentBindMethod, string parentFieldName)
        {
            CodeGenerator = codeGenerator;
            ParentBindMethod = parentBindMethod;
            ParentFieldName = parentFieldName;
        }
    }
}
