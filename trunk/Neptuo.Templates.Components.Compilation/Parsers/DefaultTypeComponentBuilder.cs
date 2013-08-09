using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class DefaultTypeComponentBuilder : BaseTypeComponentBuilder
    {
        protected Type Type { get; private set; }

        public DefaultTypeComponentBuilder(Type type)
        {
            Type = type;
        }

        protected override Type GetControlType(XmlElement element)
        {
            return Type;
        }
    }
}
