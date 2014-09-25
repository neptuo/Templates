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
        protected ObserverLivecycle Scope { get; private set; }

        public DefaultTypeObserverBuilder(Type type, ObserverLivecycle scope)
        {
            Type = type;
            Scope = scope;
        }

        protected override Type GetObserverType(IEnumerable<IXmlAttribute> attributes)
        {
            return Type;
        }

        protected override ObserverLivecycle GetObserverScope(IContentBuilderContext context, IEnumerable<IXmlAttribute> attributes)
        {
            return Scope;
        }
    }
}
