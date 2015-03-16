using Neptuo.Activators;
using Neptuo.ComponentModel;
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
        public string Name { get; private set; }
        public IParserService ParserService { get; private set; }

        public DefaultParserContext(string name, IParserService parserService, IParserServiceContext context)
            : base(context.DependencyProvider, context.Errors)
        {
            Ensure.NotNull(name, "name");
            Ensure.NotNull(parserService, "parserService");
            Name = name;
            ParserService = parserService;
        }

        public DefaultParserContext(string name, IDependencyProvider dependencyProvider, IParserService parserService, ICollection<IErrorInfo> errors)
            : base(dependencyProvider, errors)
        {
            Ensure.NotNull(name, "name");
            Ensure.NotNull(parserService, "parserService");
            Name = name;
            ParserService = parserService;
        }
    }
}
