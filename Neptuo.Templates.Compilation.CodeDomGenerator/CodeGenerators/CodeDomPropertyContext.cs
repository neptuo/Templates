using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class CodeDomPropertyContext
    {
        public CodeDomGenerator CodeGenerator { get; private set; }
        public CodeDomGenerator.Context Context { get; private set; }
        public string FieldName { get; private set; }
        public CodeMemberMethod BindMethod { get; private set; }

        public CodeDomPropertyContext(CodeDomGenerator.Context context, string fieldName, CodeMemberMethod bindMethod)
        {
            CodeGenerator = context.CodeGenerator;
            Context = context;
            FieldName = fieldName;
            BindMethod = bindMethod;
        }
    }
}
