using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers
{
    public interface IObserverBuilder
    {
        void Parse(IContentBuilderContext context, IComponentCodeObject codeObject, IEnumerable<IXmlAttribute> attributes);
    }

    public enum ObserverBuilderScope
    {
        PerAttribute, PerElement, PerDocument
    }
}
