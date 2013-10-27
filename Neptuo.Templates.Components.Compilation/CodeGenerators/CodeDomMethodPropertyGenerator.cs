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
            context.Statements.Add(
                new CodeMethodInvokeExpression(
                    new CodeFieldReferenceExpression(
                        null,
                        context.FieldName
                    ),
                    propertyDescriptor.Method.Name,
                    propertyDescriptor.Parameters.Select(
                        p => context.CodeGenerator.GenerateCodeObject(
                            new CodeObjectExtensionContext(context.Context, context.FieldName), p, propertyDescriptor
                        )
                    ).ToArray()
                )
            );
        }
    }
}
