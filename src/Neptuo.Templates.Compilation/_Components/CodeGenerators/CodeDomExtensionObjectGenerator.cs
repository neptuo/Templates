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
    public class CodeDomExtensionObjectGenerator : TypeCodeDomObjectGenerator<ComponentCodeObject>
    {
        public CodeDomExtensionObjectGenerator(IFieldNameProvider fieldNameProvider, ComponentManagerDescriptor componentManager)
            : base(fieldNameProvider, componentManager)
        { }

        protected override CodeExpression GenerateCode(CodeObjectExtensionContext context, ComponentCodeObject codeObject, ICodeProperty codeProperty)
        {
            CodeExpression field = GenerateCompoment(context, codeObject);
            
            CodeExpression result = new CodeMethodInvokeExpression(
                field,
                ComponentManager.ProvideValeExtensionMethodName,
                CreateExtensionContext(context, codeObject, codeProperty)
            );

            if (RequiredConverter(codeObject, codeProperty))
            {
                result = new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(
                        new CodeThisReferenceExpression(),
                        CodeDomStructureGenerator.Names.CastValueToMethod,
                        new CodeTypeReference(codeProperty.Property.Type)
                    ),
                    result
                );
            }
            else
            {
                result = new CodeCastExpression(codeProperty.Property.Type, result);
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

        protected bool RequiredConverter(ComponentCodeObject codeObject, ICodeProperty codeProperty)
        {
            if (codeProperty.Property.Type.IsAssignableFrom(typeof(object)))
                return false;

            ReturnTypeAttribute attribute = ReflectionHelper.GetAttribute<ReturnTypeAttribute>(codeObject.Type);
            if (attribute != null)
                return !codeProperty.Property.Type.IsAssignableFrom(attribute.Type);

            return true;
        }

        protected CodeExpression CreateExtensionContext(CodeObjectExtensionContext context, ComponentCodeObject codeObject, ICodeProperty codeProperty)
        {
            CodePrimitiveExpression propertyExpression = null;
            if (codeProperty.Property != null)
                propertyExpression = new CodePrimitiveExpression(codeProperty.Property.Name);
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
