using Neptuo.Linq.Expressions;
using Neptuo.Models.Features;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.CodeObjects.Features;
using Neptuo.Templates.Compilation.Parsers.Descriptors;
using Neptuo.Templates.Compilation.Parsers.Descriptors.Features;
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
            ComponentCodeObject codeObject = new ComponentCodeObject();
            codeObject
                .Add<ITypeCodeObject>(new TypeCodeObject(GetControlType(element)))
                .Add<IFieldCollectionCodeObject>(new FieldCollectionCodeObject());

            return codeObject;
        }

        protected override IComponentDescriptor GetComponentDescriptor(IContentBuilderContext context, ICodeObject codeObject, IXmlElement element)
        {
            Type controlType = GetControlType(element);

            DefaultComponentDescriptor component = new DefaultComponentDescriptor();
            component
                .Add<IFieldEnumerator>(new TypePropertyFieldEnumerator(controlType));
                // TODO: Add default property
                //.Add<IDefaultFieldEnumerator>(new TypePropertyFieldEnumerator;

            return component;
        }
    }
}
