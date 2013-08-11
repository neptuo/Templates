﻿using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Neptuo.Templates.Compilation.Parsers
{
    public abstract class BaseTypeObserverBuilder : BaseObserverBuilder
    {
        protected abstract Type GetObserverType(IEnumerable<XmlAttribute> attributes);
        protected abstract ObserverLivecycle GetObserverScope(IContentBuilderContext context, IEnumerable<XmlAttribute> attributes);

        protected override IObserverCodeObject CreateCodeObject(IContentBuilderContext context, IEnumerable<XmlAttribute> attributes)
        {
            return new ObserverCodeObject(GetObserverType(attributes), GetObserverScope(context, attributes));
        }

        protected override IObserverInfo GetObserverDefinition(IContentBuilderContext context, IComponentCodeObject codeObject, IEnumerable<XmlAttribute> attributes)
        {
            return new TypeInfo(GetObserverType(attributes));
        }

        protected override IPropertyDescriptor CreateSetPropertyDescriptor(IPropertyInfo propertyInfo)
        {
            return new SetPropertyDescriptor(propertyInfo);
        }

        protected override IPropertyDescriptor CreateListAddPropertyDescriptor(IPropertyInfo propertyInfo)
        {
            return new ListAddPropertyDescriptor(propertyInfo);
        }

        protected override void ProcessUnboundAttributes(IContentBuilderContext context, IObserverCodeObject codeObject, List<XmlAttribute> unboundAttributes)
        {
            ITypeCodeObject typeCodeObject = codeObject as ITypeCodeObject;
            if (typeCodeObject != null)
            {
                foreach (XmlAttribute attribute in unboundAttributes)
                    BaseBuilder.BindAttributeCollection(context, typeCodeObject, codeObject, attribute.LocalName, attribute.Value);
            }
            else
            {
                throw new NotImplementedException("Can't process unbound attributes!");
            }
        }
    }
}
