using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class BaseTypeObserverRegistration : IObserverRegistration
    {
        protected Type ObserverType { get; private set; }
        public ObserverBuilderScope Scope { get; set; }
        public ObserverLivecycle Livecycle { get; set; }

        public BaseTypeObserverRegistration(Type observerType, ObserverBuilderScope scope, ObserverLivecycle livecycle)
        {
            ObserverType = observerType;
            Scope = scope;
            Livecycle = livecycle;
        }

        public IObserverBuilder CreateBuilder()
        {
            return new DefaultTypeObserverBuilder(ObserverType, Livecycle);
        }
    }

    public class DefaultTypeObserverRegistration : BaseTypeObserverRegistration
    {
        public DefaultTypeObserverRegistration(Type observerType, ObserverBuilderScope scope)
            : base(observerType, scope, GetLivecycleFromScope(scope))
        { }

        public DefaultTypeObserverRegistration(Type observerType, ObserverLivecycle livecycle)
            : base(observerType, GetScopeFromLivecycle(livecycle), livecycle)
        { }

        protected static ObserverLivecycle GetLivecycleFromScope(ObserverBuilderScope scope)
        {
            switch (scope)
            {
                case ObserverBuilderScope.PerAttribute:
                    return ObserverLivecycle.PerAttribute;
                case ObserverBuilderScope.PerElement:
                    return ObserverLivecycle.PerControl;
                case ObserverBuilderScope.PerDocument:
                    return ObserverLivecycle.PerPage;
            }
            return ObserverLivecycle.PerControl;
        }

        protected static ObserverBuilderScope GetScopeFromLivecycle(ObserverLivecycle livecycle)
        {
            switch (livecycle)
            {
                case ObserverLivecycle.PerAttribute:
                    return ObserverBuilderScope.PerAttribute;
                case ObserverLivecycle.PerControl:
                    return ObserverBuilderScope.PerElement;
                case ObserverLivecycle.PerPage:
                    return ObserverBuilderScope.PerDocument;
            }
            return ObserverBuilderScope.PerElement;
        }
    }
}
