using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Content parser context.
    /// </summary>
    public interface IContentParserContext : IParserServiceContext
    {
        /// <summary>
        /// Current parser service.
        /// </summary>
        IParserService ParserService { get; }
    }
}
