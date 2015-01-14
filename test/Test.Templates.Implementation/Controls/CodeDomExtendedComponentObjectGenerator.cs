using Neptuo.ComponentModel;
using Neptuo.Linq.Expressions;
using Neptuo.Reflection;
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
    public class CodeDomExtendedComponentObjectGenerator : ICodeDomComponentGenerator
    {
        private readonly ICodeDomComponentGenerator componentGenerator;
        private readonly ICodeDomComponentGenerator extensionGenerator;

        public CodeDomExtendedComponentObjectGenerator(ICodeDomComponentGenerator componentGenerator, ICodeDomComponentGenerator extensionGenerator)
        {
            Guard.NotNull(componentGenerator, "componentGenerator");
            Guard.NotNull(extensionGenerator, "extensionGenerator");
            this.componentGenerator = componentGenerator;
            this.extensionGenerator = extensionGenerator;
        }

        private bool IsExtension(ITypeCodeObject codeObject)
        {
            return typeof(IValueExtension).IsAssignableFrom(codeObject.Type);
        }

        public CodeExpression GenerateCode(CodeObjectExtensionContext context, ICodeObject codeObject, ICodeProperty codeProperty)
        {
            if (IsExtension(codeObject as ITypeCodeObject))
                return extensionGenerator.GenerateCode(context, codeObject, codeProperty);
            else
                return componentGenerator.GenerateCode(context, codeObject, codeProperty);
        }
    }

    public class CodeDomExtendedComponentObjectGenerator2 : CodeDomComponentGenerator
    {
        public CodeDomExtendedComponentObjectGenerator2(IFieldNameProvider fieldNameProvider, ComponentManagerDescriptor componentManager)
            : base(fieldNameProvider, componentManager)
        { }

        private bool IsExtension(ITypeCodeObject codeObject)
        {
            return typeof(IValueExtension).IsAssignableFrom(codeObject.Type);
        }

        protected override CodeExpression GenerateCode(CodeObjectExtensionContext context, ComponentCodeObject codeObject, ICodeProperty codeProperty)
        {
            CodeExpression field = base.GenerateCode(context, codeObject, codeProperty);

            if (IsExtension(codeObject))
            {
                CodeExpression result = new CodeMethodInvokeExpression(
                    field,
                    ComponentManager.ProvideValeExtensionMethodName,
                    CreateExtensionContext(context, codeObject, codeProperty)
                );

                if (IsConverterRequired(codeObject, codeProperty))
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

            return field;
        }

        protected override CodeExpression GenerateCompoment<TCodeObject>(CodeObjectExtensionContext context, TCodeObject codeObject)
        {
            if (IsExtension(codeObject as ComponentCodeObject))
            {
                string fieldName = GenerateFieldName();
                ComponentMethodInfo createMethod = GenerateCreateMethod(context, codeObject, fieldName);
                return GenerateComponentReturnExpression(context, codeObject, createMethod);
            }

            return base.GenerateCompoment(context, codeObject);
        }

        protected override void AppendToCreateMethod<TCodeObject>(CodeObjectExtensionContext context, TCodeObject codeObject, ComponentMethodInfo createMethod)
        {
            base.AppendToCreateMethod<TCodeObject>(context, codeObject, createMethod);

            if (IsExtension(codeObject as ITypeCodeObject))
                GenerateBindMethodStatements(context, codeObject, createMethod);
        }

        protected override void AppendToComponentManager<TCodeObject>(CodeObjectExtensionContext context, TCodeObject codeObject, ComponentMethodInfo createMethod)
        {
            if (IsExtension(codeObject as ITypeCodeObject))
                return;

            base.AppendToComponentManager(context, codeObject, createMethod);
        }

        protected bool IsConverterRequired(ComponentCodeObject codeObject, ICodeProperty codeProperty)
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
            //        new CodePrimitiveExpression(codeProperty.Property.Name)
            //    ),
            //    new CodeFieldReferenceExpression(
            //        new CodeThisReferenceExpression(),
            //        CodeDomStructureGenerator.Names.DependencyProviderField
            //    )
            //);
        }
    }
}
