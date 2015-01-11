using Neptuo.ComponentModel;
using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Implementation of <see cref="IParserServiceContext"/>.
    /// </summary>
    public class DefaultParserServiceContext : IParserServiceContext
    {
        public IDependencyProvider DependencyProvider { get; private set; }
        public ICollection<IErrorInfo> Errors { get; private set; }

        public DefaultParserServiceContext(IDependencyProvider dependencyProvider, ICollection<IErrorInfo> errors = null)
        {
            Guard.NotNull(dependencyProvider, "dependencyProvider");
            DependencyProvider = dependencyProvider;
            Errors = errors ?? new List<IErrorInfo>();
        }

        public IContentParserContext CreateContentContext(IParserService service)
        {
            Guard.NotNull(service, "service");
            return new DefaultParserContext(service, this);
        }

        public IValueParserContext CreateValueContext(IParserService service)
        {
            Guard.NotNull(service, "service");
            return new DefaultParserContext(service, this);
        }
    }
}
