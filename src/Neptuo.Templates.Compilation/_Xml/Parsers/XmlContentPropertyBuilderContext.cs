using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Default implementation of <see cref="IXmlContentPropertyBuilderContext"/>.
    /// </summary>
    public class XmlContentPropertyBuilderContext : PropertyBuilderContext, IXmlContentPropertyBuilderContext
    {
        public IXmlContentBuilderContext BuilderContext { get; private set; }
        public Dictionary<string, object> CustomValues { get; private set; }
        public XmlContentParser Parser { get { return BuilderContext.Parser; } }

        public XmlContentPropertyBuilderContext(IXmlContentBuilderContext builderContext, IPropertyInfo propertyInfo)
            : base(builderContext.ParserContext.Name, builderContext.ParserContext, builderContext.ParserContext.ParserService, propertyInfo)
        {
            Guard.NotNull(builderContext, "builderContext");
            BuilderContext = builderContext;
        }
    }
}
