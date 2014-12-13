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

        public TypeComponentBuilder(IPropertyBuilder propertyFactory)
            : base(propertyFactory)
        { }

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
                //TODO: Realize using observer!
                BuilderBase.BindAttributeCollection(context, typeCodeObject, CodeObject, attribute.LocalName, attribute.GetValue());
                return true;
            }

            return base.ProcessUnboundAttribute(context, attribute);
        }
    }
}
