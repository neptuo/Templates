using Neptuo.Linq.Expressions;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Extensions;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators.Extensions.CodeDom
{
    public class ExtensionCodeObjectExtension : BaseComponentCodeObjectExtension<ExtensionCodeObject>
    {
        protected override CodeExpression GenerateCode(CodeObjectExtensionContext context, ExtensionCodeObject codeObject, IPropertyDescriptor propertyDescriptor)
        {
            CodeFieldReferenceExpression field = GenerateCompoment(context, codeObject, codeObject);
            
            context.ParentBindMethod.Statements.Add(
                new CodeMethodInvokeExpression(
                    new CodeThisReferenceExpression(),
                    context.CodeGenerator.FormatBindMethod(field.FieldName)
                )
            );
            
            CodeMemberField parentField = FindParentField(context);

            return new CodeCastExpression(
                propertyDescriptor.PropertyName.Type,
                new CodeMethodInvokeExpression(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(),
                        field.FieldName
                    ),
                    TypeHelper.MethodName<IValueExtension, IValueExtensionContext, object>(m => m.ProvideValue),
                    new CodeObjectCreateExpression(
                        typeof(DefaultMarkupExtensionContext),
                        new CodeFieldReferenceExpression(
                            new CodeThisReferenceExpression(),
                            parentField.Name
                        ),
                        new CodeMethodInvokeExpression(
                            new CodeTypeOfExpression(parentField.Type),
                            TypeHelper.MethodName<Type, string, PropertyInfo>(t => t.GetProperty),
                            new CodePrimitiveExpression(propertyDescriptor.PropertyName.Name)
                        ),
                        new CodeFieldReferenceExpression(
                            new CodeThisReferenceExpression(),
                            CodeDomGenerator.Names.DependencyProviderField
                        )
                    )
                )
            );
        }

        private CodeMemberField FindParentField(CodeObjectExtensionContext context)
        {
            foreach (CodeTypeMember member in context.CodeDomContext.Class.Members)
            {
                if (member is CodeMemberField && member.Name == context.ParentFieldName)
                    return (CodeMemberField)member;
            }
            return null;
        }
    }
}
