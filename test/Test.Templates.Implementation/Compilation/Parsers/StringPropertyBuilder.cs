using Neptuo.Templates.Compilation;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Templates.Compilation.Parsers
{
    public class StringPropertyBuilder : IPropertyBuilder
    {
        public IEnumerable<ICodeProperty> TryParse(IPropertyBuilderContext context, ISourceContent value)
        {
            ICodeProperty codeProperty = new SetCodeProperty(context.PropertyInfo);
            ICodeObject valueObject = context.ParserService.ProcessValue(value, context.CreateValueContext(context.ParserService));
            if (valueObject != null)
            {
                codeProperty.SetValue(valueObject);
                return new CodePropertyList(codeProperty);
            }

            return null;
        }
    }

}
