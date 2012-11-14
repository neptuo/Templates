using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Neptuo.Web.Framework.Compilation.Parsers
{
    public interface IXmlControlBuilder
    {
        void Parse(XmlContentParser.Helper helper, Type controlType, XmlElement element);
    }
}
