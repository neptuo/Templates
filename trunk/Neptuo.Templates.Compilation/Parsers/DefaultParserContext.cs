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
    public class DefaultParserContext : IContentParserContext, IValueParserContext
    {
        public IDependencyProvider DependencyProvider { get; private set; }
        public IParserService ParserService { get; private set; }
        public ICollection<IErrorInfo> Errors { get; private set; }

        public DefaultParserContext(IParserService parserService, IParserServiceContext context)
        {
            Guard.NotNull(parserService, "parserService");
            Guard.NotNull(context, "context");
            DependencyProvider = context.DependencyProvider;
            Errors = context.Errors;
            ParserService = parserService;
        }

        public DefaultParserContext(IDependencyProvider dependencyProvider, IParserService parserService, ICollection<IErrorInfo> errors = null)
        {
            Guard.NotNull(dependencyProvider, "dependencyProvider");
            Guard.NotNull(parserService, "parserService");
            DependencyProvider = dependencyProvider;
            ParserService = parserService;
            Errors = errors ?? new List<IErrorInfo>();
        }
    }
}
