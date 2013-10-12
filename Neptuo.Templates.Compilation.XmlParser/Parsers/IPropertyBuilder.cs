using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public interface IPropertyBuilder
    {
        bool Parse(IContentBuilderContext context, IPropertiesCodeObject codeObject, IPropertyInfo propertyInfo, IEnumerable<IXmlNode> content);

        bool Parse(IContentBuilderContext context, IPropertiesCodeObject codeObject, IPropertyInfo propertyInfo, string attributeValue);
    }
}
