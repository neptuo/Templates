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
    public static class _TextParserServiceExtensions
    {
        /// <summary>
        /// Registers <paramref name="contentParser"/> with <paramref name="name"/>.
        /// This parser will be inserted to the index 0.
        /// </summary>
        /// <param name="parserService">Parser service to extend.</param>
        /// <param name="name">Name of parser.</param>
        /// <param name="contentParser">Content parser.</param>
        public static TextParserService AddContentParser(this TextParserService parserService, string name, ITextContentParser contentParser)
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
        public static TextParserService AddValueParser(this TextParserService parserService, string name, ITextValueParser valueParser)
        {
            Ensure.NotNull(parserService, "parserService");
            parserService.GetValueParsers(name).Insert(0, valueParser);
            return parserService;
        }
    }
}
