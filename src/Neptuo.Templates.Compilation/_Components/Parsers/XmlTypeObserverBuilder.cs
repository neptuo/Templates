using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public abstract class XmlTypeObserverBuilder : XmlObserverDescriptorBuilder
    {
        protected abstract Type GetObserverType(IXmlAttribute attribute);

        protected override ICodeObject CreateCodeObject(IXmlContentBuilderContext context, IXmlAttribute attribute)
        {
            return new ObserverCodeObject(GetObserverType(attribute));
        }

        protected override IComponentDescriptor GetObserverDescriptor(IXmlContentBuilderContext context, IObserversCodeObject codeObject, IXmlAttribute attribute)
        {
            return new TypeComponentDescriptor(GetObserverType(attribute));
        }

        protected override ICodeObject IsObserverContained(IXmlContentBuilderContext context, IObserversCodeObject codeObject, IXmlAttribute attribute)
        {
            Type observerType = GetObserverType(attribute);
            return codeObject.Observers.OfType<ITypeCodeObject>().FirstOrDefault(o => o.Type == observerType);
        }
    }
}
