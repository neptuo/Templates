using Neptuo.Web.Framework.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.Parsers
{
    public class DefaultParserContext : IContentParserContext, IValueParserContext
    {
        public IDependencyProvider DependencyProvider { get; private set; }
        public IParserService ParserService { get; private set; }
        public IPropertyDescriptor PropertyDescriptor { get; private set; }

        public DefaultParserContext(IDependencyProvider dependencyProvider, IParserService generatorService, IPropertyDescriptor propertyDescriptor)
        {
            DependencyProvider = dependencyProvider;
            ParserService = generatorService;
            PropertyDescriptor = propertyDescriptor;
        }
    }
}
