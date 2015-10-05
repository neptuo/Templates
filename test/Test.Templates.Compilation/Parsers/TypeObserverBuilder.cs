using Neptuo.Models.Features;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.CodeObjects.Features;
using Neptuo.Templates.Compilation.Parsers.Descriptors;
using Neptuo.Templates.Compilation.Parsers.Descriptors.Features;
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
        protected abstract Type GetObserverType(IXmlAttribute attribute);

        protected override IComponentCodeObject CreateCodeObject(IContentBuilderContext context, IXmlAttribute attribute)
        {
            ObserverCodeObject codeObject = new ObserverCodeObject();
            codeObject
                .Add<ITypeCodeObject>(new TypeCodeObject(GetObserverType(attribute)))
                .Add<IFieldCollectionCodeObject>(new FieldCollectionCodeObject());

            return codeObject;
        }

        protected override IComponentDescriptor GetObserverDescriptor(IContentBuilderContext context, IComponentCodeObject codeObject, IXmlAttribute attribute)
        {
            Type observerType = GetObserverType(attribute);

            DefaultComponentDescriptor component = new DefaultComponentDescriptor();
            component
                .Add<IFieldEnumerator>(new TypePropertyFieldEnumerator(observerType))
                .Add<IDefaultFieldEnumerator>(new DefaultPropertyFieldEnumerator(observerType));

            return component;
        }

        protected override IComponentCodeObject IsObserverContained(IContentBuilderContext context, IComponentCodeObject codeObject, IXmlAttribute attribute)
        {
            Type observerType = GetObserverType(attribute);
            ITypeCodeObject typeCodeObject;
            return codeObject.With<IObserverCollectionCodeObject>().EnumerateObservers().OfType<IComponentCodeObject>().FirstOrDefault(o => o.TryWith(out typeCodeObject) && typeCodeObject.Type == observerType);
        }
    }
}
