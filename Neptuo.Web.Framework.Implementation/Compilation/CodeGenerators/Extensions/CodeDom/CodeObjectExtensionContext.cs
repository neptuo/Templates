using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.CodeGenerators.Extensions.CodeDom
{
    public class CodeObjectExtensionContext
    {
        public IDependencyProvider DependencyProvider { get; private set; }
        public CodeDomGenerator CodeGenerator { get; private set; }
        public CodeMemberMethod ParentBindMethod { get; private set; }
        public string ParentFieldName { get; private set; }

        public CodeObjectExtensionContext(IDependencyProvider dependencyProvider, CodeDomGenerator codeGenerator, CodeMemberMethod parentBindMethod, string parentFieldName)
        {
            DependencyProvider = dependencyProvider;
            CodeGenerator = codeGenerator;
            ParentBindMethod = parentBindMethod;
            ParentFieldName = parentFieldName;
        }
    }
}
