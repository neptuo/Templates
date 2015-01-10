using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class RootContentBuilder : ComponentDescriptorBuilder
    {
        private readonly IPropertyInfo defaultProperty;

        public RootContentBuilder(IPropertyBuilder propertyFactory, IObserverBuilder observerFactory, IPropertyInfo defaultProperty)
            : base(propertyFactory, observerFactory)
        {
            Guard.NotNull(defaultProperty, "defaultProperty");
            this.defaultProperty = defaultProperty;
        }

        protected override IComponentCodeObject CreateCodeObject(IContentBuilderContext context, IXmlElement element)
        {
            return new RootCodeObject();
        }

        protected override IComponentDescriptor GetComponentDescriptor(IContentBuilderContext context, IComponentCodeObject codeObject, IXmlElement element)
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
