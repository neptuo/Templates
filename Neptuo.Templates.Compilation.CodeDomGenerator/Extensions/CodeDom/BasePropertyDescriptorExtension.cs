using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators.Extensions.CodeDom
{
    public abstract class BasePropertyDescriptorExtension<T> : IPropertyDescriptorExtension
        where T : IPropertyDescriptor
    {
        public void GenerateProperty(PropertyDescriptorExtensionContext context, IPropertyDescriptor propertyDescriptor)
        {
            GenerateProperty(context, (T)propertyDescriptor);
        }

        protected abstract void GenerateProperty(PropertyDescriptorExtensionContext context, T propertyDescriptor);

        protected CodeExpression GetPropertyTarget(PropertyDescriptorExtensionContext context)
        {
            if (context.FieldName != null)
            {
                return new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(),
                    context.FieldName
                );
            }
                
            return new CodeThisReferenceExpression();
        }
    }
}
