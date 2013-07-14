using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Neptuo.Templates.Compilation.Parsers
{
    public interface IObserverBuilder
    {
        void Parse(IBuilderContext context, IComponentCodeObject codeObject, IEnumerable<XmlAttribute> attributes);
    }
}
