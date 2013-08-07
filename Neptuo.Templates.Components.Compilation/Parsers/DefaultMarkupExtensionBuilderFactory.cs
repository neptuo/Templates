using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class DefaultMarkupExtensionBuilderFactory : IMarkupExtensionBuilderFactory
    {
        protected Type MarkupExtensionType { get; private set; }

        public DefaultMarkupExtensionBuilderFactory(Type markupExtensionType)
        {
            MarkupExtensionType = markupExtensionType;
        }

        public IMarkupExtensionBuilder CreateBuilder(string prefix, string name)
        {
            return new DefaultMarkupExtensionBuilder(MarkupExtensionType);
        }
    }
}
