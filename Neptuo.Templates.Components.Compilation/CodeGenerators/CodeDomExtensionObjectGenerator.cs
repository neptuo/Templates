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
            CodeExpression field = GenerateCompoment(context, codeObject);
            
            return new CodeCastExpression(
                propertyDescriptor.Property.Type,
                new CodeMethodInvokeExpression(
                    field,
                    TypeHelper.MethodName<IValueExtension, IValueExtensionContext, object>(m => m.ProvideValue),
                    new CodeObjectCreateExpression(
                        typeof(DefaultMarkupExtensionContext),
                        new CodeFieldReferenceExpression(
                            null,
                            context.ParentFieldName
                        ),
                        new CodeMethodInvokeExpression(
                            new CodeMethodInvokeExpression(
                                new CodeFieldReferenceExpression(
                                    null,
                                    context.ParentFieldName
                                ),
                                TypeHelper.MethodName<object, Type>(t => t.GetType)
                            ),
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

        protected override CodeExpression GenerateCompoment<TCodeObject>(CodeObjectExtensionContext context, TCodeObject codeObject)
        {
            string fieldName = GenerateFieldName();
            ComponentMethodInfo createMethod = GenerateCreateMethod(context, codeObject, fieldName);
            return GenerateComponentReturnExpression(context, codeObject, createMethod);
        }

        protected override void AppendToCreateMethod<TCodeObject>(CodeObjectExtensionContext context, TCodeObject codeObject, ComponentMethodInfo createMethod)
        {
            base.AppendToCreateMethod<TCodeObject>(context, codeObject, createMethod);
            GenerateBindMethodStatements(context, codeObject, createMethod);
        }

        protected override void AppendToComponentManager<TCodeObject>(CodeObjectExtensionContext context, TCodeObject codeObject, ComponentMethodInfo createMethod)
        { }
    }
}
