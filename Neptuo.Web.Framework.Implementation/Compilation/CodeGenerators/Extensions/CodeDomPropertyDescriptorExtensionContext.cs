using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.CodeGenerators.Extensions
{
    public class CodeDomPropertyDescriptorExtensionContext
    {
        public CodeDomGenerator CodeGenerator { get; private set; }
        public string FieldName { get; private set; }
        public CodeMemberMethod BindMethod { get; private set; }

        public CodeDomPropertyDescriptorExtensionContext(CodeDomGenerator codeGenerator, string fieldName, CodeMemberMethod bindMethod)
        {
            CodeGenerator = codeGenerator;
            FieldName = fieldName;
            BindMethod = bindMethod;
        }
    }
}
