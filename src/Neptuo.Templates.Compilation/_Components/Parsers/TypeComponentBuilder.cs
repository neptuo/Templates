using Neptuo.Linq.Expressions;
using Neptuo.Models.Features;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.CodeObjects.Features;
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

        protected override ICodeObject CreateCodeObject(IContentBuilderContext context, IXmlElement element)
        {
            ComponentCodeObject codeObject = new ComponentCodeObject();
            codeObject
                .Add<ITypeCodeObject>(new TypeCodeObject(GetControlType(element)))
                .Add<IFieldCollectionCodeObject>(new FieldCollectionCodeObject());

            return codeObject;
        }

        protected override IXComponentDescriptor GetComponentDescriptor(IContentBuilderContext context, ICodeObject codeObject, IXmlElement element)
        {
            return new TypeComponentDescriptor(GetControlType(element));
        }
    }
}
