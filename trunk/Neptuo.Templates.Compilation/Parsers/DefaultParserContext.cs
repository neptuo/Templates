using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Implementation of <see cref="IContentParserContext"/> and <see cref="IValueParserContext"/>.
    /// </summary>
    public class DefaultParserContext : DefaultParserServiceContext, IContentParserContext, IValueParserContext
    {
        public IParserService ParserService { get; private set; }

        public DefaultParserContext(IParserService parserService, IParserServiceContext context)
            : base(context.DependencyProvider, context.Errors)
        {
            Guard.NotNull(parserService, "parserService");
            ParserService = parserService;
        }

        public DefaultParserContext(IDependencyProvider dependencyProvider, IParserService parserService, ICollection<IErrorInfo> errors)
            : base(dependencyProvider, errors)
        {
            Guard.NotNull(parserService, "parserService");
            ParserService = parserService;
        }
    }
}
