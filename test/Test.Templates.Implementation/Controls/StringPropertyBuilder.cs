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
    public class StringPropertyBuilder : TypeDefaultPropertyBuilder
    {
        public override bool TryParse(IContentParserContext context, IPropertiesCodeObject codeObject, IPropertyInfo propertyInfo, ISourceContent attributeValue)
        {
            IPropertyDescriptor propertyDescriptor = new SetPropertyDescriptor(propertyInfo);
            ICodeObject valueObject = context.ParserService.ProcessValue(attributeValue, context.CreateValueContext(context.ParserService));
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
