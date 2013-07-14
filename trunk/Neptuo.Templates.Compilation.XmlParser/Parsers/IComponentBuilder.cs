using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Neptuo.Templates.Compilation.Parsers
{
    public interface IComponentBuilder
    {
        void Parse(IBuilderContext context, XmlElement element);
    }
}
