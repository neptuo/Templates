using Neptuo.ComponentModel;
using Neptuo.Linq.Expressions;
using Neptuo.Reflection;
using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class CodeDomExtensionObjectGenerator : TypeCodeDomObjectGenerator<ExtensionCodeObject>
    {
        public CodeDomExtensionObjectGenerator(IFieldNameProvider fieldNameProvider, ComponentManagerDescriptor componentManager)
            : base(fieldNameProvider, componentManager)
        { }

        protected override CodeExpression GenerateCode(CodeObjectExtensionContext context, ExtensionCodeObject codeObject, IPropertyDescriptor propertyDescriptor)
        {
            CodeExpression field = GenerateCompoment(context, codeObject);
            
            CodeExpression result = new CodeMethodInvokeExpression(
                field,
                ComponentManager.ProvideValeExtensionMethodName,
                CreateTokenContext(context, codeObject, propertyDescriptor)
            );

            if (RequiredConverter(codeObject, propertyDescriptor))
            {
                result = new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(
                        new CodeThisReferenceExpression(),
                        CodeDomStructureGenerator.Names.CastValueToMethod,
                        new CodeTypeReference(propertyDescriptor.Property.Type)
                    ),
                    result
                );
            }
            else
            {
                result = new CodeCastExpression(propertyDescriptor.Property.Type, result);
            }

            return result;
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
        
        protected bool RequiredConverter(ExtensionCodeObject codeObject, IPropertyDescriptor propertyDescriptor)
        {
            if (propertyDescriptor.Property.Type.IsAssignableFrom(typeof(object)))
                return false;

            ReturnTypeAttribute attribute = ReflectionHelper.GetAttribute<ReturnTypeAttribute>(codeObject.Type);
            if (attribute != null)
                return !propertyDescriptor.Property.Type.IsAssignableFrom(attribute.Type);

            return true;
        }

        protected CodeExpression CreateTokenContext(CodeObjectExtensionContext context, ExtensionCodeObject codeObject, IPropertyDescriptor propertyDescriptor)
        {
            CodePrimitiveExpression propertyExpression = null;
            if (propertyDescriptor.Property != null)
                propertyExpression = new CodePrimitiveExpression(propertyDescriptor.Property.Name);
            else
                propertyExpression = new CodePrimitiveExpression(null);

            return new CodeMethodInvokeExpression(
                new CodeThisReferenceExpression(),
                CodeDomStructureGenerator.Names.CreateValueExtensionContextMethod,
                new CodeFieldReferenceExpression(null, context.ParentFieldName),
                propertyExpression
            );
            //return new CodeObjectCreateExpression(
            //    typeof(DefaultTokenContext),
            //    new CodeFieldReferenceExpression(
            //        null,
            //        context.ParentFieldName
            //    ),
            //    new CodeMethodInvokeExpression(
            //        new CodeMethodInvokeExpression(
            //            new CodeFieldReferenceExpression(
            //                null,
            //                context.ParentFieldName
            //            ),
            //            TypeHelper.MethodName<object, Type>(t => t.GetType)
            //        ),
            //        TypeHelper.MethodName<Type, string, PropertyInfo>(t => t.GetProperty),
            //        new CodePrimitiveExpression(propertyDescriptor.Property.Name)
            //    ),
            //    new CodeFieldReferenceExpression(
            //        new CodeThisReferenceExpression(),
            //        CodeDomStructureGenerator.Names.DependencyProviderField
            //    )
            //);
        }
    }
}
