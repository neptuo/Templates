using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neptuo.Web.Framework.Parser.HtmlContent;
using System.Xml;

namespace Neptuo.Web.Framework.Compilation
{
    public interface IXmlControlBuilder
    {
        void GenerateControl(Type controlType, XmlElement source, XmlBuilderContext context);
    }
}
