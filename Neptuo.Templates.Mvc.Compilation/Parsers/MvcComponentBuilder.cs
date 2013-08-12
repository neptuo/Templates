using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class MvcComponentBuilder : BaseComponentBuilder
    {
        protected override IComponentCodeObject CreateCodeObject(IContentBuilderContext context, XmlElement element)
        {
            throw new NotImplementedException();
        }

        protected override IComponentInfo GetComponentDefinition(IContentBuilderContext context, IComponentCodeObject codeObject, XmlElement element)
        {
            throw new NotImplementedException();
        }

        protected override IPropertyDescriptor CreateSetPropertyDescriptor(IPropertyInfo propertyInfo)
        {
            throw new NotImplementedException();
        }

        protected override IPropertyDescriptor CreateListAddPropertyDescriptor(IPropertyInfo propertyInfo)
        {
            throw new NotImplementedException();
        }

        protected override void ProcessUnboundAttribute(IContentBuilderContext context, IComponentCodeObject codeObject, XmlAttribute unboundAttribute)
        {
            
        }
    }
}
