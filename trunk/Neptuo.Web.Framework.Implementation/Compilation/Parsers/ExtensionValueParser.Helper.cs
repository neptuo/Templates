using Neptuo.Web.Framework.Parser.ExtensionContent;
using Neptuo.Web.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.Parsers
{
    partial class ExtensionValueParser
    {
        public class Helper
        {
            public IValueParserContext Context { get; protected set; }
            public IRegistrator Registrator { get; protected set; }
            public ExtensionContentParser Parser { get; protected set; }

            public Helper(IValueParserContext context)
            {
                Context = context;
                Registrator = Context.DependencyProvider.Resolve<IRegistrator>();
                Parser = new ExtensionContentParser();
            }
        }
    }
}
