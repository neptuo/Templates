using Neptuo.Web.Framework.Parser.ExtensionContent;
using Neptuo.Web.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    partial class ExtensionValueParser
    {
        public class Helper
        {
            public IParserContext Context { get; protected set; }
            public IRegistrator Registrator { get; protected set; }
            public ExtensionContentParser Parser { get; protected set; }

            public Helper(IParserContext context)
            {
                Context = context;
                Registrator = Context.ServiceProvider.GetService<IRegistrator>();
                Parser = new ExtensionContentParser();
            }
        }
    }
}
