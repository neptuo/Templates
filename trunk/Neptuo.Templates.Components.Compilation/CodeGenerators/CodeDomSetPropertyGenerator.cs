using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class CodeDomSetPropertyGenerator : BaseCodeDomPropertyGenerator<SetPropertyDescriptor>
    {
        protected override void GenerateProperty(CodeDomPropertyContext context, SetPropertyDescriptor propertyDescriptor)
        {
            CodeExpression codeExpression = context.CodeGenerator.GenerateCodeObject(context.Context, propertyDescriptor.Value, propertyDescriptor, context.BindMethod, context.FieldName);
            context.BindMethod.Statements.Add(
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
