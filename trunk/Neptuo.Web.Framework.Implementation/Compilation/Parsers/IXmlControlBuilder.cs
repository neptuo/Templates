using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Neptuo.Web.Framework.Compilation.Parsers
{
    public interface IXmlControlBuilder
    {
        void Parse(IXmlBuilderContext context, Type controlType, XmlElement element);
    }
}
