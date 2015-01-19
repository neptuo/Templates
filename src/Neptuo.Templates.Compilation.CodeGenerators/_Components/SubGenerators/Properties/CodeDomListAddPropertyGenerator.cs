﻿using Neptuo.Linq.Expressions;
using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class CodeDomListAddPropertyGenerator : CodeDomPropertyGeneratorBase<ListAddCodeProperty>
    {
        private static string addMethodName = TypeHelper.MethodName<ICollection<object>, object>(c => c.Add);

        protected override ICodeDomPropertyResult Generate(ICodeDomPropertyContext context, ListAddCodeProperty codeProperty)
        {
            bool isGenericProperty = codeProperty.Property.Type.IsGenericType;
            bool isCastingRequired = false;
            bool isWriteable = !codeProperty.Property.IsReadOnly;
            Type targetType = null;

            CodeExpression targetField = context.PropertyTarget;
            CodeExpression codePropertyReference = new CodePropertyReferenceExpression(
                targetField,
                codeProperty.Property.Name
            );

            DefaultCodeDomPropertyResult statements = new DefaultCodeDomPropertyResult();
            if (typeof(IEnumerable).IsAssignableFrom(codeProperty.Property.Type))
            {
                isCastingRequired = true;
                if (isGenericProperty)
                {
                    targetType = typeof(List<>).MakeGenericType(codeProperty.Property.Type.GetGenericArguments()[0]);

                    if (typeof(ICollection<>).IsAssignableFrom(codeProperty.Property.Type.GetGenericTypeDefinition()))
                        isCastingRequired = false;
                }
                else
                {
                    targetType = typeof(List<object>);
                }
            }

            if (isWriteable)
            {
                statements.AddStatement(
                    new CodeAssignStatement(
                        codePropertyReference,
                        new CodeObjectCreateExpression(targetType)
                    )
                );
            }

            if (isCastingRequired)
                codePropertyReference = new CodeCastExpression(targetType, codePropertyReference);

            foreach (ICodeObject propertyValue in codeProperty.Values)
            {
                ICodeDomObjectResult result = context.Registry.WithObjectGenerator().Generate(
                    context.CreateObjectContext().AddCodeProperty(codeProperty),
                    propertyValue
                );

                if (result == null)
                    return null;

                if (result.HasExpression())
                {
                    statements.AddStatement(
                        new CodeExpressionStatement(
                            new CodeMethodInvokeExpression(
                                codePropertyReference,
                                addMethodName,
                                result.Expression
                            )
                        )
                    );
                }
                //TODO: Other bindable ways
            }

            return statements;
        }
    }
}
