using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class TypeDefaultPropertyBuilder : DefaultPropertyBuilder
    {
        protected bool IsCollectionProperty(IPropertyInfo propertyInfo)
        {
            return typeof(IEnumerable).IsAssignableFrom(propertyInfo.Type);
            //TODO: Test for IEnumerable should be enough.
            //return typeof(ICollection).IsAssignableFrom(propertyInfo.Type)
            //    || (propertyInfo.Type.IsGenericType && typeof(ICollection<>).IsAssignableFrom(propertyInfo.Type.GetGenericTypeDefinition()));
        }

        protected override ICodeProperty CreateCodeProperty(IPropertyInfo propertyInfo)
        {
            ICodeProperty codeProperty = null;
            if (IsCollectionProperty(propertyInfo))
                codeProperty = new ListAddCodeProperty(propertyInfo);
            else
                codeProperty = new SetCodeProperty(propertyInfo);

            return codeProperty;
        }
    }
}
