using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class CodeDomRootGenerator : CodeDomObjectGeneratorBase<RootCodeObject>
    {
        protected override CodeExpression GenerateCode(CodeObjectExtensionContext context, RootCodeObject codeObject, IPropertyDescriptor propertyDescriptor)
        {
            foreach (IPropertyDescriptor propertyDesc in codeObject.Properties)
            {
                context.CodeGenerator.GenerateProperty(
                    new CodeDomPropertyContext(context.CodeDomContext, null, context.BaseStructure.EntryPointMethod.Statements),
                    propertyDesc
                );
            }
            return null;
        }
    }
}
