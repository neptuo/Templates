using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class XmlDefaultTypeComponentBuilder : XmlTypeComponentBuilder
    {
        protected Type Type { get; private set; }

        public XmlDefaultTypeComponentBuilder(Type type)
        {
            Type = type;
        }

        protected override Type GetControlType(IXmlElement element)
        {
            return Type;
        }
    }
}
