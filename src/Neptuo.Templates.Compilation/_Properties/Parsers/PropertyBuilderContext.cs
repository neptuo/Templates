using Neptuo.ComponentModel;
using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Default implementation of <see cref="IPropertyBuilderContext"/>
    /// </summary>
    public class PropertyBuilderContext : IPropertyBuilderContext
    {
        private readonly IParserServiceContext context;

        public IParserService ParserService { get; private set; }
        public IPropertyInfo PropertyInfo { get; private set; }
        public IDependencyProvider DependencyProvider { get { return context.DependencyProvider; } }
        public ICollection<IErrorInfo> Errors { get { return context.Errors; } }

        public PropertyBuilderContext(IParserServiceContext context, IParserService parserService, IPropertyInfo propertyInfo)
        {
            Guard.NotNull(context, "context");
            Guard.NotNull(parserService, "parserService");
            Guard.NotNull(propertyInfo, "propertyInfo");
            this.context = context;
            ParserService = parserService;
            PropertyInfo = propertyInfo;
        }

        public IContentParserContext CreateContentContext(IParserService service)
        {
            return context.CreateContentContext(service);
        }

        public IValueParserContext CreateValueContext(IParserService service)
        {
            return context.CreateValueContext(service);
        }
    }
}
