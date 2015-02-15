using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.XmlDocuments
{
    public class RootContentBuilder : ComponentDescriptorBuilder
    {
        private readonly IPropertyInfo defaultProperty;

        public RootContentBuilder(IPropertyInfo defaultProperty)
        {
            Guard.NotNull(defaultProperty, "defaultProperty");
            this.defaultProperty = defaultProperty;
        }

        protected override ICodeObject CreateCodeObject(IContentBuilderContext context, IXmlElement element)
        {
            return new RootCodeObject();
        }

        protected override IComponentDescriptor GetComponentDescriptor(IContentBuilderContext context, ICodeObject codeObject, IXmlElement element)
        {
            return new RootComponentDescriptor(defaultProperty);
        }

        protected override bool TryBindProperty(IContentBuilderContext context, string prefix, string name, IEnumerable<IXmlNode> value)
        {
            return base.TryBindProperty(context, prefix, name, value);
        }

        protected override bool ProcessUnboundAttribute(IContentBuilderContext context, IXmlAttribute unboundAttribute)
        {
            return true;
        }
    }
}
