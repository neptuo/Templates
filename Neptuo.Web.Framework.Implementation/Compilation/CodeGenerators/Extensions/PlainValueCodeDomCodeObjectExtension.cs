using Neptuo.Web.Framework.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.CodeGenerators.Extensions
{
    public class PlainValueCodeDomCodeObjectExtension : BaseCodeDomCodeObjectExtension<IPlainValueCodeObject>
    {
        protected override CodeExpression GenerateCode(CodeDomCodeObjectExtensionContext context, IPlainValueCodeObject plainValue, IPropertyDescriptor propertyDescriptor)
        {
            return new CodePrimitiveExpression(plainValue.Value);
            //return null;
        }
    }
}
