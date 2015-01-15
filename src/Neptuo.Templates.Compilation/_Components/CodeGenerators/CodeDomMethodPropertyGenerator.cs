using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class CodeDomMethodPropertyGenerator : CodeDomPropertyGeneratorBase<MethodInvokeCodeProperty>
    {
        public CodeDomMethodPropertyGenerator(Type requiredComponentType)
            : base(requiredComponentType)
        { }

        protected override void GenerateProperty(CodeDomPropertyContext context, MethodInvokeCodeProperty codeProperty)
        {
            context.Statements.Add(
                new CodeMethodInvokeExpression(
                    new CodeFieldReferenceExpression(
                        null,
                        context.FieldName
                    ),
                    codeProperty.Method.Name,
                    codeProperty.Parameters.Select(
                        p => context.CodeGenerator.GenerateCodeObject(
                            new CodeObjectExtensionContext(context.Context, context.FieldName), p, codeProperty
                        )
                    ).Where(p => p != null).ToArray()
                )
            );
        }
    }
}
