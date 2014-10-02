using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleNG.Controls
{
    public class StringPropertyBuilder : IPropertyBuilder
    {
        public bool Parse(IContentBuilderContext context, IPropertiesCodeObject codeObject, IPropertyInfo propertyInfo, IEnumerable<IXmlNode> content)
        {
            throw new NotImplementedException();
        }

        public bool Parse(IContentBuilderContext context, IPropertiesCodeObject codeObject, IPropertyInfo propertyInfo, string attributeValue)
        {
            IPropertyDescriptor propertyDescriptor = new SetPropertyDescriptor(propertyInfo);
            ICodeObject valueObject = context.ParserContext.ParserService.ProcessValue(
                attributeValue,
                new DefaultParserServiceContext(context.ParserContext.DependencyProvider, context.ParserContext.Errors)
            );

            if (valueObject != null)
            {
                propertyDescriptor.SetValue(valueObject);
                codeObject.Properties.Add(propertyDescriptor);
                return true;
            }

            return false;
        }
    }

    public class StringPropertyBuilderFactory : IPropertyBuilderFactory
    {
        public IPropertyBuilder CreateBuilder(IPropertyInfo propertyInfo)
        {
            return new StringPropertyBuilder();
        }
    }

}
