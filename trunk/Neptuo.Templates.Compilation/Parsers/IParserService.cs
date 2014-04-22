using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers
{
    public interface IParserService
    {
        IList<IContentParser> ContentParsers { get; }
        IList<IValueParser> ValueParsers { get; }
        IValueParser DefaultValueParser { get; set; }

        event Func<string, IParserServiceContext, bool> OnSearchContentParser;
        event Func<string, IParserServiceContext, bool> OnSearchValueParser;
        event Func<string, IParserServiceContext, bool> OnSearchDefaultValueParser;

        bool ProcessContent(string content, IParserServiceContext context);
        bool ProcessValue(string value, IParserServiceContext context);
    }
}