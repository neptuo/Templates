using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Neptuo.Templates.Compilation.Parsers
{
    public abstract class BaseComponentBuilder : IComponentBuilder
    {
        public void Parse(IBuilderContext context, XmlElement element)
        {
            IComponentCodeObject codeObject = CreateCodeObject(context, element);
            BindProperties(context, codeObject, element);
            AppendToParent(context, codeObject);
        }

        protected virtual void AppendToParent(IBuilderContext context, IComponentCodeObject codeObject)
        {
            context.Parent.SetValue(codeObject);
        }

        protected abstract IComponentCodeObject CreateCodeObject(IBuilderContext context, XmlElement element);

        protected abstract void BindProperties(IBuilderContext context, IComponentCodeObject codeObject, XmlElement element);

        protected abstract void ProcessUnboundAttributes(IBuilderContext context, IComponentCodeObject codeObject, List<XmlAttribute> unboundAttributes);
    }
}
