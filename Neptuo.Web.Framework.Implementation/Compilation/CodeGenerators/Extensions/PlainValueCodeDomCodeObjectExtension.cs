using Neptuo.Web.Framework.Compilation.CodeObjects;
using Neptuo.Web.Framework.Utils;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.CodeGenerators.Extensions
{
    public class PlainValueCodeDomCodeObjectExtension : BaseCodeDomCodeObjectExtension<IPlainValueCodeObject>
    {
        protected override CodeExpression GenerateCode(CodeDomCodeObjectExtensionContext context, IPlainValueCodeObject plainValue, IPropertyDescriptor propertyDescriptor)
        {
            if (Utils.StringConverter.CanConvert(propertyDescriptor.Property.PropertyType))
                return new CodePrimitiveExpression(Utils.StringConverter.Convert(plainValue.Value.ToString(), propertyDescriptor.Property.PropertyType));

            TypeConverter typeConverter = null;
            TypeConverterAttribute attribute = ReflectionHelper.GetAttribute<TypeConverterAttribute>(propertyDescriptor.Property);
            if (attribute != null)
                typeConverter = (TypeConverter)Activator.CreateInstance(Type.GetType(attribute.ConverterTypeName));
            else
                typeConverter = TypeDescriptor.GetConverter(propertyDescriptor.Property.PropertyType);

            if (typeConverter != null && typeConverter.CanConvertTo(propertyDescriptor.Property.PropertyType))
            {
                return new CodeCastExpression(
                    propertyDescriptor.Property.PropertyType,
                    new CodeMethodInvokeExpression(
                        new CodeObjectCreateExpression(typeConverter.GetType()),
                        "ConvertTo",
                        //TypeHelper.MethodName<TypeConverter, object, Type, object>(t => t.ConvertTo),
                        new CodePrimitiveExpression(plainValue.Value),
                        new CodeTypeOfExpression(propertyDescriptor.Property.PropertyType)
                    )
                );
            }

            //TODO: Inconvertable value!!
            return new CodePrimitiveExpression(plainValue.Value);
        }
    }
}
