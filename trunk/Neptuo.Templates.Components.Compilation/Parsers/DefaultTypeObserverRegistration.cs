using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class DefaultTypeObserverRegistration : IObserverRegistration
    {
        protected Type ObserverType { get; private set; }
        public ObserverBuilderScope Scope { get; set; }

        public DefaultTypeObserverRegistration(Type observerType, ObserverBuilderScope scope)
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
