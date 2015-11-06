﻿using Neptuo.Linq.Expressions;
using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Generator for properties which should use <see cref="Dictionary{K,V}.Add"/> method.
    /// </summary>
    public class CodeDomMapPropertyGenerator : CodeDomPropertyGeneratorBase<MapCodeProperty>
    {
        /// <summary>
        /// Name of the <see cref="Dictionary{K,V}.Add"/> method.
        /// </summary>
        private static string addMethodName = TypeHelper.MethodName<IDictionary<string, string>, string, string>(d => d.Add);

        protected override ICodeDomPropertyResult Generate(ICodeDomPropertyContext context, MapCodeProperty codeProperty)
        {
            CodeDomDefaultPropertyResult statements = new CodeDomDefaultPropertyResult();

            bool isWriteable = false;
            Type targetKeyItemType = typeof(object);
            Type targetValueItemType = typeof(object);

            // Expression for accessing target property.
            CodeExpression targetField = context.PropertyTarget;
            CodeExpression codePropertyReference = new CodePropertyReferenceExpression(
                targetField,
                codeProperty.Name
            );

            Type declaringType;
            if (context.TryGetFieldType(out declaringType))
            {
                PropertyInfo propertyInfo = declaringType.GetProperty(codeProperty.Name);
                if (propertyInfo != null && propertyInfo.GetSetMethod() != null)
                    isWriteable = propertyInfo.CanWrite;
            }

            // Try to get target property type.
            MethodInfo methodInfo = codeProperty.Type.GetMethod(addMethodName);
            if (methodInfo == null)
            {
                context.AddError(
                    "Unnable to bind property of type '{0}' using 'DictionaryAddCodeProperty' because it doesn't have Add method with two parameters.",
                    codeProperty.Type.FullName
                );
                return null;
            }

            ParameterInfo[] parameters = methodInfo.GetParameters();
            if (parameters.Length != 2)
            {
                context.AddError(
                    "Unnable to bind property of type '{0}' using 'DictionaryAddCodeProperty' because it doesn't have Add method with two parameters.",
                    codeProperty.Type.FullName
                );
                return null;
            }

            targetKeyItemType = parameters[0].ParameterType;
            targetValueItemType = parameters[1].ParameterType;

            if (codeProperty.Type.IsGenericType)
            {
                Type[] genericArguments = codeProperty.Type.GetGenericArguments();
                if (genericArguments.Length == 2)
                {
                    targetKeyItemType = genericArguments[0];
                    targetValueItemType = genericArguments[1];
                }
            }

            // If writeable, create new instance.
            if (isWriteable)
            {
                statements.AddStatement(
                    new CodeAssignStatement(
                        codePropertyReference,
                        new CodeObjectCreateExpression(codeProperty.Type)
                    )
                );
            }

            // Foreach property value...
            foreach (KeyValuePair<ICodeObject, ICodeObject> propertyValue in codeProperty.Values)
            {
                // Resolve key code object value.
                ICodeDomObjectResult keyResult = context.Registry.WithObjectGenerator().Generate(
                    context.CreateObjectContext().AddCodeProperty(codeProperty).AddPropertyTarget(context.PropertyTarget),
                    propertyValue.Key
                );

                // Resolve value code object value.
                ICodeDomObjectResult valueResult = context.Registry.WithObjectGenerator().Generate(
                    context.CreateObjectContext().AddCodeProperty(codeProperty).AddPropertyTarget(context.PropertyTarget),
                    propertyValue.Value
                );

                // If result is null, something wrong.
                if (keyResult == null || valueResult == null)
                    return null;

                // If result has expression...
                if (keyResult.HasExpression() && valueResult.HasExpression())
                {
                    // Try to convert to key type.
                    CodeExpression keyExpression = context.Registry.WithConversionGenerator().Generate(
                        context,
                        targetKeyItemType,
                        keyResult.Expression,
                        keyResult.Expression.GetReturnType()
                    );

                    // Try to convert to value type.
                    CodeExpression valueExpression = context.Registry.WithConversionGenerator().Generate(
                        context,
                        targetValueItemType,
                        valueResult.Expression,
                        valueResult.Expression.GetReturnType()
                    );

                    if (keyExpression == null || valueExpression == null)
                        return null;

                    // Add statement to the collection.
                    statements.AddStatement(
                        new CodeExpressionStatement(
                            new CodeMethodInvokeExpression(
                                codePropertyReference,
                                addMethodName,
                                keyExpression,
                                valueExpression
                            )
                        )
                    );
                }
                else
                {
                    // If key result has statement (possibly comment), add it to the result.
                    if (keyResult.HasStatement())
                        statements.AddStatement(keyResult.Statement);

                    // If value result has statement (possibly comment), add it to the result.
                    if(valueResult.HasStatement())
                        statements.AddStatement(valueResult.Statement);
                }
            }
            return statements;
        }
    }
}