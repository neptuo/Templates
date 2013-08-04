using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class CodeDomMethodPropertyGenerator : BaseCodeDomPropertyGenerator<MethodInvokePropertyDescriptor>
    {
        protected override void GenerateProperty(CodeDomPropertyContext context, MethodInvokePropertyDescriptor propertyDescriptor)
        {
            context.BindMethod.Statements.Add(
                new CodeMethodInvokeExpression(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(),
                        context.FieldName
                    ),
                    propertyDescriptor.Method.Name,
                    propertyDescriptor.Parameters.Select(p => new CodePrimitiveExpression(((PlainValueCodeObject)p).Value)).ToArray()
                )
            );
        }
    }
}
