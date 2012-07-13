using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Parser
{
    public interface IContentParser
    {
        bool Parse(string content);
    }

    public interface IContentParser<T> : IContentParser
    {
        OnParsedItem<T> OnParsedItem { get; set; }

        OnParserDone OnParserDone { get; set; }
    }

    public delegate void OnParsedItem<T>(ContentParserEventArgs<T> e);

    public delegate void OnParserDone(ContentParserDoneEventArgs e);
}
