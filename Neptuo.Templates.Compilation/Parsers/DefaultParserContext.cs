using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class DefaultParserContext : IContentParserContext, IValueParserContext
    {
        public IDependencyProvider DependencyProvider { get; private set; }
        public IParserService ParserService { get; private set; }
        public IPropertyDescriptor PropertyDescriptor { get; private set; }
        public ICollection<IErrorInfo> Errors { get; private set; }

        public DefaultParserContext(IParserServiceContext context, IParserService parserService)
        {
            DependencyProvider = context.DependencyProvider;
            PropertyDescriptor = context.PropertyDescriptor;
            Errors = context.Errors;
            ParserService = parserService;
        }

        public DefaultParserContext(IDependencyProvider dependencyProvider, IParserService parserService, IPropertyDescriptor propertyDescriptor, ICollection<IErrorInfo> errors = null)
        {
            DependencyProvider = dependencyProvider;
            ParserService = parserService;
            PropertyDescriptor = propertyDescriptor;
            Errors = errors ?? new List<IErrorInfo>();
        }
    }
}
