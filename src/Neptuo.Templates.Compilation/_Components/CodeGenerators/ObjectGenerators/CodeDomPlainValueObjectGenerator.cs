using Neptuo.Linq.Expressions;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class CodeDomPlainValueObjectGenerator : XCodeDomObjectGeneratorBase<IPlainValueCodeObject>
    {
        protected override CodeExpression GenerateCode(CodeObjectExtensionContext context, IPlainValueCodeObject plainValue, ICodeProperty codeProperty)
        {
            if (codeProperty.Property == null || plainValue  == null)
                return null;

            if (codeProperty.Property.CanAssign(plainValue.Value.GetType()))
                return new CodePrimitiveExpression(plainValue.Value);

            TypeConverter typeConverter = GetTypeConverter(context, plainValue, codeProperty);
            if (typeConverter != null && typeConverter.CanConvertFrom(plainValue.Value.GetType()))
            {
                object value = typeConverter.ConvertFrom(plainValue.Value);
                if(value == null)
                    return new CodePrimitiveExpression(null);

                if (value.GetType().IsEnum)
                {
                    return new CodePropertyReferenceExpression(
                        new CodeVariableReferenceExpression(value.GetType().FullName), 
                        value.ToString()
                    );
                }

                return new CodePrimitiveExpression(value);
            }
            else if (codeProperty.Property != null && StringConverter.CanConvert(codeProperty.Property.Type))
            {
                return new CodePrimitiveExpression(StringConverter.Convert(plainValue.Value.ToString(), codeProperty.Property.Type));
            }

            if (plainValue.Value is string)
            {
                string textValue = (string)plainValue.Value;
                if (String.IsNullOrEmpty(textValue) || String.IsNullOrWhiteSpace(textValue))
                    return null;
            }

            //TODO: Inconvertable value!!
            //Be aware of ListAddPropertyDescriptor - type of it is Collection<TARGET REAL TYPE>
            return new CodePrimitiveExpression(plainValue.Value);
            //throw new InvalidOperationException(
            //    String.Format(
            //        "Unnable to convert to target type! Source type: {0}, target type: {1}",
            //        plainValue.Value.GetType(),
            //        propertyDescriptor.Property.Type
            //    )
            //);
        }

        protected TypeConverter GetTypeConverter(CodeObjectExtensionContext context, IPlainValueCodeObject plainValue, ICodeProperty codeProperty)
        {
            //TypeConverter typeConverter = null;
            //TypeConverterAttribute attribute = ReflectionHelper.GetAttribute<TypeConverterAttribute>();
            //if(attribute == null)
            //TypeConverterAttribute attribute = ReflectionHelper.GetAttribute<TypeConverterAttribute>(propertyDescriptor.Property.Type);
            //if (attribute != null)
            //{
            //    typeConverter = (TypeConverter)context.CodeDomContext.CodeGeneratorContext.DependencyProvider.Resolve(Type.GetType(attribute.ConverterTypeName), null);
            //    getConverterExpression = new CodeCastExpression(
            //        typeof(TypeDescriptor),
            //        new CodeMethodInvokeExpression(
            //            new CodeFieldReferenceExpression(
            //                new CodeThisReferenceExpression(),
            //                BaseStructureExtension.Names.DependencyProviderField
            //            ),
            //            TypeHelper.MethodName<IDependencyProvider, Type, string, object>(d => d.Resolve)
            //        )
            //    );
            //}
            if (codeProperty.Property == null)
                return null;

            return TypeDescriptor.GetConverter(codeProperty.Property.Type);
        }
    }
}
