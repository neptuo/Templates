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

        protected override ICodeObject CreateCodeObject(IContentBuilderContext context, IXmlElement element)
        {
            return new ComponentCodeObject(GetControlType(element));
        }

        protected override IComponentDescriptor GetComponentDescriptor(IContentBuilderContext context, ICodeObject codeObject, IXmlElement element)
        {
            return new TypeComponentDescriptor(GetControlType(element));
        }
    }
}
