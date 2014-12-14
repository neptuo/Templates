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
        public TypeObserverBuilder(ObserverBuilderScope scope, IPropertyBuilder propertyFactory)
            : base(scope, propertyFactory)
        { }

        protected abstract Type GetObserverType(IXmlAttribute attribute);

        protected ObserverLivecycle GetObserverScope(IContentBuilderContext context, IXmlAttribute attribute)
        {
            switch (Scope)
            {
                case ObserverBuilderScope.PerAttribute:
                    return ObserverLivecycle.PerAttribute;
                case ObserverBuilderScope.PerElement:
                    return ObserverLivecycle.PerControl;
                case ObserverBuilderScope.PerDocument:
                    return ObserverLivecycle.PerPage;
                default:
                    throw new NotSupportedException(Scope.ToString());
            }
        }

        protected override IObserverCodeObject CreateCodeObject(IContentBuilderContext context, IXmlAttribute attribute)
        {
            return new ObserverCodeObject(GetObserverType(attribute), GetObserverScope(context, attribute));
        }

        protected override IObserverDescriptor GetObserverDescriptor(IContentBuilderContext context, IComponentCodeObject codeObject, IXmlAttribute attribute)
        {
            return new TypeDescriptorBase(GetObserverType(attribute));
        }

        protected override IObserverCodeObject IsObserverContained(IContentBuilderContext context, IComponentCodeObject codeObject, IXmlAttribute attribute)
        {
            Type observerType = GetObserverType(attribute);
            return codeObject.Observers.FirstOrDefault(o => o.Type == observerType);
        }
    }
}
