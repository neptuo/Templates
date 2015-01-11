using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class DefaultTypeObserverBuilder : TypeObserverBuilder
    {
        protected Type Type { get; private set; }

        public DefaultTypeObserverBuilder(Type type, IPropertyBuilder propertyFactory)
            : base(propertyFactory)
        {
            Type = type;
        }

        protected override Type GetObserverType(IXmlAttribute attribute)
        {
            return Type;
        }
    }
}
