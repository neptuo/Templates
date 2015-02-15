using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class XmlRootContentBuilder : XmlComponentDescriptorBuilder
    {
        private readonly IPropertyInfo defaultProperty;

        public XmlRootContentBuilder(IPropertyInfo defaultProperty)
        {
            Guard.NotNull(defaultProperty, "defaultProperty");
            this.defaultProperty = defaultProperty;
        }

        protected override ICodeObject CreateCodeObject(IXmlContentBuilderContext context, IXmlElement element)
        {
            return new RootCodeObject();
        }

        protected override IComponentDescriptor GetComponentDescriptor(IXmlContentBuilderContext context, ICodeObject codeObject, IXmlElement element)
        {
            return new RootComponentDescriptor(defaultProperty);
        }

        protected override bool TryBindProperty(IXmlContentBuilderContext context, string prefix, string name, IEnumerable<IXmlNode> value)
        {
            return base.TryBindProperty(context, prefix, name, value);
        }

        protected override bool ProcessUnboundAttribute(IXmlContentBuilderContext context, IXmlAttribute unboundAttribute)
        {
            return true;
        }
    }
}
