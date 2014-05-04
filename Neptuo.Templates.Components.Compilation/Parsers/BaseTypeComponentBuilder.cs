using Neptuo.Linq.Expressions;
using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public abstract class BaseTypeComponentBuilder : BaseComponentBuilder
    {
        protected abstract Type GetControlType(IXmlElement element);

        protected override IComponentCodeObject CreateCodeObject(IContentBuilderContext context, IXmlElement element)
        {
            return new ComponentCodeObject(GetControlType(element));
        }

        protected override IComponentInfo GetComponentDefinition(IContentBuilderContext context, IComponentCodeObject codeObject, IXmlElement element)
        {
            return new TypeInfo(GetControlType(element));
        }

        protected override void ProcessUnboundAttribute(IContentBuilderContext context, IComponentCodeObject codeObject, IXmlAttribute attribute)
        {
            ITypeCodeObject typeCodeObject = codeObject as ITypeCodeObject;
            if (typeCodeObject != null)
                BaseBuilder.BindAttributeCollection(context, typeCodeObject, codeObject, attribute.LocalName, attribute.Value);
            else
                throw new NotImplementedException(String.Format("Can't process unbound attributes! Attribute {0} on {1}.", attribute.Name, attribute.OwnerElement.Name));
        }

        protected override IPropertyDescriptor CreateListAddPropertyDescriptor(IPropertyInfo propertyInfo)
        {
            return new ListAddPropertyDescriptor(propertyInfo);
        }

        protected override IPropertyDescriptor CreateSetPropertyDescriptor(IPropertyInfo propertyInfo)
        {
            return new SetPropertyDescriptor(propertyInfo);
        }
    }
}
