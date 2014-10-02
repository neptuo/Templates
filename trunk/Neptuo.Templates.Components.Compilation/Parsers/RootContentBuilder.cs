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
        protected override void ProcessUnboundAttribute(IContentBuilderContext context, IXmlAttribute unboundAttribute)
        { }

        protected override IComponentCodeObject CreateCodeObject(IContentBuilderContext context, IXmlElement element)
        {
            return new RootCodeObject();
        }

        protected override IComponentDescriptor GetComponentDescriptor(IContentBuilderContext context, IComponentCodeObject codeObject, IXmlElement element)
        {
            return new RootComponentDescriptor();
        }

        protected override IPropertyDescriptor CreatePropertyDescriptor(IPropertyInfo propertyInfo)
        {
            return new ListAddPropertyDescriptor(ComponentDefinition.GetDefaultProperty());
        }
    }
}
