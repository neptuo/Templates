using Neptuo.Linq.Expressions;
using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators.Extensions.CodeDom
{
    public class PlainValueCodeObjectExtension : BaseCodeObjectExtension<IPlainValueCodeObject>
    {
        protected override CodeExpression GenerateCode(CodeObjectExtensionContext context, IPlainValueCodeObject plainValue, IPropertyDescriptor propertyDescriptor)
        {
            if (StringConverter.CanConvert(propertyDescriptor.Property.PropertyType))
                return new CodePrimitiveExpression(StringConverter.Convert(plainValue.Value.ToString(), propertyDescriptor.Property.PropertyType));

            TypeConverter typeConverter = null;
            TypeConverterAttribute attribute = ReflectionHelper.GetAttribute<TypeConverterAttribute>(propertyDescriptor.Property);
            if(attribute == null)
                attribute = ReflectionHelper.GetAttribute<TypeConverterAttribute>(propertyDescriptor.Property.PropertyType);

            CodeExpression getConverterExpression = null;

            //typeConverter = TypeDescriptor.GetConverter(propertyDescriptor.Property.PropertyType);

            if (attribute != null)
            {
                typeConverter = (TypeConverter)context.CodeDomContext.CodeGeneratorContext.DependencyProvider.Resolve(Type.GetType(attribute.ConverterTypeName), null);
                getConverterExpression = new CodeCastExpression(
                    typeof(TypeDescriptor),
                    new CodeMethodInvokeExpression(
                        new CodeFieldReferenceExpression(
                            new CodeThisReferenceExpression(),
                            CodeDomGenerator.Names.DependencyProviderField
                        ),
                        TypeHelper.MethodName<IDependencyProvider, Type, string, object>(d => d.Resolve)
                    )
                );
            }

            if (typeConverter == null)
            {
                typeConverter = TypeDescriptor.GetConverter(propertyDescriptor.Property.PropertyType);
                getConverterExpression = new CodeMethodInvokeExpression(
                    new CodeTypeReferenceExpression(typeof(TypeDescriptor)),
                    "GetConverter",
                    new CodeTypeOfExpression(propertyDescriptor.Property.PropertyType)
                );
            }

            //Type propertyType = propertyDescriptor.Property.PropertyType;
            //if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            //    propertyType = propertyType.GetGenericArguments()[0];

            if (typeConverter != null && typeConverter.CanConvertFrom(typeof(String)))
            {
                return new CodeCastExpression(
                    propertyDescriptor.Property.PropertyType,
                    new CodeMethodInvokeExpression(
                        getConverterExpression,
                        TypeHelper.MethodName<TypeConverter, object, object>(t => t.ConvertFrom),
                        new CodePrimitiveExpression(plainValue.Value)
                    )
                );
            }

            //TODO: Inconvertable value!!
            return new CodePrimitiveExpression(plainValue.Value);
        }
    }
}
