using Neptuo.Templates.Compilation;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Templates.Controls
{
    public class StringPropertyBuilder : IPropertyBuilder
    {
        public bool TryParse(IContentBuilderContext context, IPropertiesCodeObject codeObject, IPropertyInfo propertyInfo, IEnumerable<IXmlNode> content)
        {
            throw new NotImplementedException();
        }

        public bool TryParse(IContentBuilderContext context, IPropertiesCodeObject codeObject, IPropertyInfo propertyInfo, ISourceContent value)
        {
            IPropertyDescriptor propertyDescriptor = new SetPropertyDescriptor(propertyInfo);
            ICodeObject valueObject = context.TryProcessValue(value);
            if (valueObject != null)
            {
                propertyDescriptor.SetValue(valueObject);
                codeObject.Properties.Add(propertyDescriptor);
                return true;
            }

            return false;
        }
    }

}
