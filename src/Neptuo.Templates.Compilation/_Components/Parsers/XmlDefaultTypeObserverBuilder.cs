using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class XmlDefaultTypeObserverBuilder : XmlTypeObserverBuilder
    {
        protected Type Type { get; private set; }

        public XmlDefaultTypeObserverBuilder(Type type)
        {
            Type = type;
        }

        protected override Type GetObserverType(IXmlAttribute attribute)
        {
            return Type;
        }
    }
}
