using Neptuo.Templates.Compilation;
using Neptuo.Templates.Compilation.Parsers;
using Neptuo.Templates.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestConsoleNG.Controls;

namespace TestConsoleNG
{
    public static class TestTemplates
    {
        public static void Test()
        {
            IBuilderRegistry registry = new TypeBuilderRegistry(
                new LiteralControlBuilder<LiteralControl>(c => c.Text), 
                new GenericContentControlBuilder<GenericContentControl>(c => c.TagName)
            );

            //TODO: Create CodeDomViewService...

            IParserService parserService = new DefaultParserService();
            XmlContentParser parser = new XmlContentParser(registry);
            parserService.ContentParsers.Add(parser);
            parserService.ProcessContent("<a href='google'>Hello!</a>", new DefaultParserServiceContext(null, null));
        }
    }
}
