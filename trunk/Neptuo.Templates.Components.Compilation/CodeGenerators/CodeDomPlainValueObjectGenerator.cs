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
    public class CodeDomPlainValueObjectGenerator : BaseCodeDomObjectGenerator<IPlainValueCodeObject>
    {
        protected override CodeExpression GenerateCode(CodeObjectExtensionContext context, IPlainValueCodeObject plainValue, IPropertyDescriptor propertyDescriptor)
        {
            TypeConverter typeConverter = GetTypeConverter(context, plainValue, propertyDescriptor);
            if (typeConverter != null && typeConverter.CanConvertFrom(plainValue.Value.GetType()))
                return new CodePrimitiveExpression(typeConverter.ConvertFrom(plainValue.Value));
            else if (StringConverter.CanConvert(propertyDescriptor.Property.Type))
                return new CodePrimitiveExpression(StringConverter.Convert(plainValue.Value.ToString(), propertyDescriptor.Property.Type));

            //TODO: Inconvertable value!!
            return new CodePrimitiveExpression(plainValue.Value);
        }

        protected TypeConverter GetTypeConverter(CodeObjectExtensionContext context, IPlainValueCodeObject plainValue, IPropertyDescriptor propertyDescriptor)
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
            return TypeDescriptor.GetConverter(propertyDescriptor.Property.Type);
        }
    }
}
