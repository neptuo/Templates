using Neptuo.Web.Framework.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    public class DefaultParserContext : IContentParserContext, IValueParserContext
    {
        public IServiceProvider ServiceProvider { get; private set; }
        public IParserService ParserService { get; private set; }
        public IPropertyDescriptor PropertyDescriptor { get; private set; }

        public DefaultParserContext(IServiceProvider serviceProvider, IParserService generatorService, IPropertyDescriptor propertyDescriptor)
        {
            ServiceProvider = serviceProvider;
            ParserService = generatorService;
            PropertyDescriptor = propertyDescriptor;
        }
    }
}
