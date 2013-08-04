using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public abstract class BaseCodeDomPropertyGenerator<T> : ICodeDomPropertyGenerator
        where T : IPropertyDescriptor
    {
        public void GenerateProperty(CodeDomPropertyContext context, IPropertyDescriptor propertyDescriptor)
        {
            GenerateProperty(context, (T)propertyDescriptor);
        }

        protected abstract void GenerateProperty(CodeDomPropertyContext context, T propertyDescriptor);

        protected CodeExpression GetPropertyTarget(CodeDomPropertyContext context)
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
