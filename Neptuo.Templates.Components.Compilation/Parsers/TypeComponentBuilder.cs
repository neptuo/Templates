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
    /// <summary>
    /// Base builder that uses <see cref="Type"/> for defining component.
    /// </summary>
    public abstract class TypeComponentBuilder : ComponentDescriptorBuilder
    {
        protected abstract Type GetControlType(IXmlElement element);

        protected override IComponentCodeObject CreateCodeObject(IContentBuilderContext context, IXmlElement element)
        {
            return new ComponentCodeObject(GetControlType(element));
        }

        protected override IComponentDescriptor GetComponentDescriptor(IContentBuilderContext context, IComponentCodeObject codeObject, IXmlElement element)
        {
            return new TypeDescriptorBase(GetControlType(element));
        }

        protected override bool ProcessUnboundAttribute(IContentBuilderContext context, IXmlAttribute attribute)
        {
            ITypeCodeObject typeCodeObject = CodeObject as ITypeCodeObject;
            if (typeCodeObject != null)
            {
                BuilderBase.BindAttributeCollection(context, typeCodeObject, CodeObject, attribute.LocalName, attribute.GetValue());
                return true;
            }

            return base.ProcessUnboundAttribute(context, attribute);
        }

        protected bool IsCollectionProperty(IPropertyInfo propertyInfo)
        {
            return typeof(IEnumerable).IsAssignableFrom(propertyInfo.Type);
            //TODO: Test for IEnumerable should be enough.
            //return typeof(ICollection).IsAssignableFrom(propertyInfo.Type)
            //    || (propertyInfo.Type.IsGenericType && typeof(ICollection<>).IsAssignableFrom(propertyInfo.Type.GetGenericTypeDefinition()));
        }

        protected override IPropertyDescriptor CreatePropertyDescriptor(IPropertyInfo propertyInfo)
        {
            IPropertyDescriptor propertyDescriptor = null;
            if (IsCollectionProperty(propertyInfo))
                propertyDescriptor = new ListAddPropertyDescriptor(propertyInfo);
            else
                propertyDescriptor = new SetPropertyDescriptor(propertyInfo);

            return propertyDescriptor;
        }
    }
}
