using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers.Descriptors;
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

        public ContentPropertyBuilderContext(IContentBuilderContext builderContext, IFieldDescriptor fieldDescriptor)
            : base(builderContext.ParserContext.Name, builderContext.ParserContext, builderContext.ParserContext.ParserService, fieldDescriptor)
        {
            Ensure.NotNull(builderContext, "builderContext");
            BuilderContext = builderContext;
        }
    }
}
