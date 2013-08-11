using Neptuo.Linq.Expressions;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Extensions;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// TODO: Use type conveter!
    /// </summary>
    public class CodeDomExtensionObjectGenerator : TypeCodeDomObjectGenerator<ExtensionCodeObject>
    {
        public CodeDomExtensionObjectGenerator(IFieldNameProvider fieldNameProvider)
            : base(fieldNameProvider)
        { }

        protected override CodeExpression GenerateCode(CodeObjectExtensionContext context, ExtensionCodeObject codeObject, IPropertyDescriptor propertyDescriptor)
        {
            CodeFieldReferenceExpression field = GenerateCompoment(context, codeObject, codeObject);
            
            context.ParentBindMethod.Statements.Add(
                new CodeMethodInvokeExpression(
                    new CodeThisReferenceExpression(),
                    FormatBindMethod(field.FieldName)
                )
            );
            
            CodeMemberField parentField = FindParentField(context);

            return new CodeCastExpression(
                propertyDescriptor.Property.Type,
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
                            new CodePrimitiveExpression(propertyDescriptor.Property.Name)
                        ),
                        new CodeFieldReferenceExpression(
                            new CodeThisReferenceExpression(),
                            CodeDomStructureGenerator.Names.DependencyProviderField
                        )
                    )
                )
            );
        }

        private CodeMemberField FindParentField(CodeObjectExtensionContext context)
        {
            foreach (CodeTypeMember member in context.BaseStructure.Class.Members)
            {
                if (member is CodeMemberField && member.Name == context.ParentFieldName)
                    return (CodeMemberField)member;
            }
            return null;
        }
    }
}
