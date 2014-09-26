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
        protected abstract Type GetObserverType(IEnumerable<IXmlAttribute> attributes);
        protected abstract ObserverLivecycle GetObserverScope(IContentBuilderContext context, IEnumerable<IXmlAttribute> attributes);

        protected override IObserverCodeObject CreateCodeObject(IContentBuilderContext context, IEnumerable<IXmlAttribute> attributes)
        {
            return new ObserverCodeObject(GetObserverType(attributes), GetObserverScope(context, attributes));
        }

        protected override IObserverDescriptor GetObserverDefinition(IContentBuilderContext context, IComponentCodeObject codeObject, IEnumerable<IXmlAttribute> attributes)
        {
            return new TypeDescriptorBase(GetObserverType(attributes));
        }

        protected override IPropertyDescriptor CreateSetPropertyDescriptor(IPropertyInfo propertyInfo)
        {
            return new SetPropertyDescriptor(propertyInfo);
        }

        protected override IPropertyDescriptor CreateListAddPropertyDescriptor(IPropertyInfo propertyInfo)
        {
            return new ListAddPropertyDescriptor(propertyInfo);
        }

        protected override void ProcessUnboundAttributes(IContentBuilderContext context, IObserverCodeObject codeObject, List<IXmlAttribute> unboundAttributes)
        {
            ITypeCodeObject typeCodeObject = codeObject as ITypeCodeObject;
            if (typeCodeObject != null)
            {
                foreach (IXmlAttribute attribute in unboundAttributes)
                    BuilderBase.BindAttributeCollection(context, typeCodeObject, codeObject, attribute.LocalName, attribute.Value);
            }
            else
            {
                throw new NotImplementedException("Can't process unbound attributes!");
            }
        }
    }
}
