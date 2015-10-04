using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Extensions for <see cref="IParserServiceContext"/>.
    /// </summary>
    public static class _ParserServiceContextExtensions
    {
        public static ITextContentParserContext CreateContentContext(this IParserServiceContext context, string name, IParserService service)
        {
            Ensure.NotNull(service, "service");
            return new TextParserContext(name, service, context);
        }

        public static ITextValueParserContext CreateValueContext(this IParserServiceContext context, string name, IParserService service)
        {
            Ensure.NotNull(service, "service");
            return new TextParserContext(name, service, context);
        }
    }
}
