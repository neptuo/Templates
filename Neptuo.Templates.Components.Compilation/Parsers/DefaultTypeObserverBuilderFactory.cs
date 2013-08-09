using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class DefaultTypeObserverBuilderFactory : IObserverBuilderFactory
    {
        protected Type ObserverType { get; private set; }
        protected ObserverBuilderScope Scope { get; private set; }

        public DefaultTypeObserverBuilderFactory(Type observerType, ObserverBuilderScope scope)
        {
            ObserverType = observerType;
            Scope = scope;
        }

        public IObserverRegistration CreateBuilder(string prefix, string attributeName)
        {
            return new DefaultTypeObserverRegistration(ObserverType, Scope);
        }
    }
}
