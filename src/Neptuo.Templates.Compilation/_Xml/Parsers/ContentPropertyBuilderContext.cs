using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Default implementation of <see cref="IContentPropertyBuilderContext"/>.
    /// </summary>
    public class ContentPropertyBuilderContext : PropertyBuilderContext, IContentPropertyBuilderContext
    {
        public IContentBuilderContext BuilderContext { get; private set; }
        public Dictionary<string, object> CustomValues { get; private set; }
        public TextXmlContentParser Parser { get { return BuilderContext.Parser; } }

        public ContentPropertyBuilderContext(IContentBuilderContext builderContext, IPropertyInfo propertyInfo)
            : base(builderContext.ParserContext.Name, builderContext.ParserContext, builderContext.ParserContext.ParserService, propertyInfo)
        {
            Ensure.NotNull(builderContext, "builderContext");
            BuilderContext = builderContext;
        }
    }
}
