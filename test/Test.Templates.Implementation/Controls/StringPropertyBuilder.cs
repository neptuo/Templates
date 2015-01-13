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
        public IEnumerable<IPropertyDescriptor> TryParse(IPropertyBuilderContext context, ISourceContent value)
        {
            IPropertyDescriptor propertyDescriptor = new SetPropertyDescriptor(context.PropertyInfo);
            ICodeObject valueObject = context.ParserService.ProcessValue(value, context.CreateValueContext(context.ParserService));
            if (valueObject != null)
            {
                propertyDescriptor.SetValue(valueObject);
                return new PropertyDescriptorList(propertyDescriptor);
            }

            return null;
        }
    }

}
