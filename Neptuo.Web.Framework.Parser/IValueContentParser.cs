using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Parser
{
    public interface IValueContentParser
    {
        bool Parse(string content);
    }

    public interface IValueContentParser<T> : IValueContentParser
    {
        OnValueParsedItem<T> OnParsedItem { get; set; }
    }

    public delegate void OnValueParsedItem<T>(ValueParserEventArgs<T> e);
}
