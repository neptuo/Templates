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

        public TypeComponentBuilder(IContentPropertyBuilder propertyFactory, IObserverBuilder observerFactory)
            : base(propertyFactory, observerFactory)
        { }

        protected override IComponentCodeObject CreateCodeObject(IContentBuilderContext context, IXmlElement element)
        {
            return new ComponentCodeObject(GetControlType(element));
        }

        protected override IComponentDescriptor GetComponentDescriptor(IContentBuilderContext context, IComponentCodeObject codeObject, IXmlElement element)
        {
            return new TypeDescriptorBase(GetControlType(element));
        }
    }
}
