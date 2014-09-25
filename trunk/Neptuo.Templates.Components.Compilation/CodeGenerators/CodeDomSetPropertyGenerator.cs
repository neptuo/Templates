using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class CodeDomSetPropertyGenerator : CodeDomPropertyGeneratorBase<SetPropertyDescriptor>
    {
        protected override void GenerateProperty(CodeDomPropertyContext context, SetPropertyDescriptor propertyDescriptor)
        {
            CodeExpression codeExpression = context.CodeGenerator.GenerateCodeObject(
                new CodeObjectExtensionContext(context.Context, context.FieldName), 
                propertyDescriptor.Value, 
                propertyDescriptor
            );
            context.Statements.Add(
                new CodeAssignStatement(
                    new CodePropertyReferenceExpression(
                        GetPropertyTarget(context),
                        propertyDescriptor.Property.Name
                    ),
                    codeExpression
                )
            );
        }
    }
}
