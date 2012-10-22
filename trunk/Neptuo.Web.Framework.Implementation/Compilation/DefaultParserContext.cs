using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    public class DefaultParserContext : IContentParserContext
    {
        public IServiceProvider ServiceProvider { get; private set; }
        public IParserService ParserService { get; private set; }
        public ICodeObject RootObject { get; private set; }

        public DefaultParserContext(IServiceProvider serviceProvider, IParserService generatorService, ICodeObject rootObject)
        {
            ServiceProvider = serviceProvider;
            ParserService = generatorService;
            RootObject = rootObject;
        }
    }
}
