using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public static class _ParserServiceExtensions
    {
        /// <summary>
        /// Registers <paramref name="contentParser"/> with <paramref name="name"/>.
        /// This parser will be inserted to the index 0.
        /// </summary>
        /// <param name="name">Name of parser.</param>
        /// <param name="contentParser">Content parser.</param>
        public static IParserService AddContentParser(this IParserService parserService, string name, IContentParser contentParser)
        {
            Guard.NotNull(parserService, "parserService");
            parserService.GetContentParsers(name).Insert(0, contentParser);
            return parserService;
        }

        /// <summary>
        /// Registers <paramref name="valueParser"/> with <paramref name="name"/>.
        /// This parser will be inserted to the index 0.
        /// </summary>
        /// <param name="name">Name of parser.</param>
        /// <param name="valueParser">Value parser.</param>
        public static IParserService AddValueParser(this IParserService parserService, string name, IValueParser valueParser)
        {
            Guard.NotNull(parserService, "parserService");
            parserService.GetValueParsers(name).Insert(0, valueParser);
            return parserService;
        }
    }
}
