using Neptuo.Linq.Expressions;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers;
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
            if (StringConverter.CanConvert(propertyDescriptor.Property.Type))
                return new CodePrimitiveExpression(StringConverter.Convert(plainValue.Value.ToString(), propertyDescriptor.Property.Type));

            
            TypeConverter typeConverter = null;
            TypeConverterAttribute attribute = ReflectionHelper.GetAttribute<TypeConverterAttribute>(((TypePropertyInfo)propertyDescriptor.Property).PropertyInfo);
            if(attribute == null)
                attribute = ReflectionHelper.GetAttribute<TypeConverterAttribute>(propertyDescriptor.Property.Type);

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
                typeConverter = TypeDescriptor.GetConverter(propertyDescriptor.Property.Type);
                getConverterExpression = new CodeMethodInvokeExpression(
                    new CodeTypeReferenceExpression(typeof(TypeDescriptor)),
                    "GetConverter",
                    new CodeTypeOfExpression(propertyDescriptor.Property.Type)
                );
            }

            //Type propertyType = propertyDescriptor.Property.PropertyType;
            //if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            //    propertyType = propertyType.GetGenericArguments()[0];

            if (typeConverter != null && typeConverter.CanConvertFrom(typeof(String)))
            {
                return new CodeCastExpression(
                    propertyDescriptor.Property.Type,
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
