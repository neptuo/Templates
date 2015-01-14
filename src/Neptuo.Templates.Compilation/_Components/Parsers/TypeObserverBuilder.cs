using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public abstract class TypeObserverBuilder : ObserverDescriptorBuilder
    {
        public TypeObserverBuilder(IPropertyBuilder propertyFactory)
            : base(propertyFactory)
        { }

        protected abstract Type GetObserverType(IXmlAttribute attribute);

        protected override IObserverCodeObject CreateCodeObject(IContentBuilderContext context, IXmlAttribute attribute)
        {
            return new ObserverCodeObject(GetObserverType(attribute));
        }

        protected override IComponentDescriptor GetObserverDescriptor(IContentBuilderContext context, IComponentCodeObject codeObject, IXmlAttribute attribute)
        {
            return new TypeComponentDescriptor(GetObserverType(attribute));
        }

        protected override IObserverCodeObject IsObserverContained(IContentBuilderContext context, IComponentCodeObject codeObject, IXmlAttribute attribute)
        {
            Type observerType = GetObserverType(attribute);
            return codeObject.Observers.FirstOrDefault(o => o.Type == observerType);
        }
    }
}
