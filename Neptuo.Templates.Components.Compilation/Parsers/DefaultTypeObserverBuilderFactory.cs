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
            return new DefaultObserverRegistration(ObserverType, Scope);
        }
    }

    public class DefaultObserverRegistration : IObserverRegistration
    {
        protected Type ObserverType { get; private set; }
        public ObserverBuilderScope Scope { get; set; }

        public DefaultObserverRegistration(Type observerType, ObserverBuilderScope scope)
        {
            ObserverType = observerType;
            Scope = scope;
        }

        public IObserverBuilder CreateBuilder()
        {
            return new DefaultTypeObserverBuilder(ObserverType, GetLivecycleFromScope());
        }

        protected virtual ObserverLivecycle GetLivecycleFromScope()
        {
            switch (Scope)
            {
                case ObserverBuilderScope.PerAttribute:
                    return ObserverLivecycle.PerAttribute;
                case ObserverBuilderScope.PerElement:
                    return ObserverLivecycle.PerControl;
            }
            return ObserverLivecycle.PerControl;
        }
    }

}
