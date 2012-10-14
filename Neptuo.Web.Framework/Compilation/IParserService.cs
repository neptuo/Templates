using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    public interface IParserService
    {
        IList<IContentParser> ContentParsers { get; }
        IList<IValueParser> ValueParsers { get; }

        bool ProcessContent(string content, IParserServiceContext context);
        bool ProcessValue(string value, IParserServiceContext context);
    }
}