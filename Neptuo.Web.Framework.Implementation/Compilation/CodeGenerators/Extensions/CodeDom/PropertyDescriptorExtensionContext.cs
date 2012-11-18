using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.CodeGenerators.Extensions.CodeDom
{
    public class PropertyDescriptorExtensionContext
    {
        public IDependencyProvider DependencyProvider { get; private set; }
        public CodeDomGenerator CodeGenerator { get; private set; }
        public string FieldName { get; private set; }
        public CodeMemberMethod BindMethod { get; private set; }

        public PropertyDescriptorExtensionContext(IDependencyProvider dependencyProvider, CodeDomGenerator codeGenerator, string fieldName, CodeMemberMethod bindMethod)
        {
            DependencyProvider = dependencyProvider;
            CodeGenerator = codeGenerator;
            FieldName = fieldName;
            BindMethod = bindMethod;
        }
    }
}
