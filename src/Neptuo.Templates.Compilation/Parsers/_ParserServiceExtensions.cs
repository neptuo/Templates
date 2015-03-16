using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Common extensions for <see cref="IParserService"/>.
    /// </summary>
    public static class _ParserServiceExtensions
    {
        /// <summary>
        /// Registers <paramref name="contentParser"/> with <paramref name="name"/>.
        /// This parser will be inserted to the index 0.
        /// </summary>
        /// <param name="parserService">Parser service to extend.</param>
        /// <param name="name">Name of parser.</param>
        /// <param name="contentParser">Content parser.</param>
        public static IParserService AddContentParser(this IParserService parserService, string name, IContentParser contentParser)
        {
            Ensure.NotNull(parserService, "parserService");
            parserService.GetContentParsers(name).Insert(0, contentParser);
            return parserService;
        }

        /// <summary>
        /// Registers <paramref name="valueParser"/> with <paramref name="name"/>.
        /// This parser will be inserted to the index 0.
        /// </summary>
        /// <param name="parserService">Parser service to extend.</param>
        /// <param name="name">Name of parser.</param>
        /// <param name="valueParser">Value parser.</param>
        public static IParserService AddValueParser(this IParserService parserService, string name, IValueParser valueParser)
        {
            Ensure.NotNull(parserService, "parserService");
            parserService.GetValueParsers(name).Insert(0, valueParser);
            return parserService;
        }
    }
}
