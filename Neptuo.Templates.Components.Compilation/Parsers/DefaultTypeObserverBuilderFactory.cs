using Neptuo.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class DefaultTypeObserverBuilderFactory : IObserverBuilderFactory
    {
        protected IObserverRegistration ObserverRegistration { get; private set; }

        public DefaultTypeObserverBuilderFactory(Type observerType, ObserverBuilderScope scope)
        {
            ObserverRegistration = new DefaultTypeObserverRegistration(observerType, scope);
        }

        public DefaultTypeObserverBuilderFactory(Type observerType, ObserverLivecycle livecycle)
        {
            ObserverRegistration = new DefaultTypeObserverRegistration(observerType, livecycle);
        }

        public DefaultTypeObserverBuilderFactory(Type observerType)
        {
            ObserverAttribute attribute = ReflectionHelper.GetAttribute<ObserverAttribute>(observerType);
            if (attribute == null)
                throw new InvalidOperationException("Unnable to find livecycle or scope for observer. Use ObserverAttribute or speciafy it explicitly!");

            ObserverRegistration = new DefaultTypeObserverRegistration(observerType, attribute.Livecycle);
        }

        public IObserverRegistration CreateBuilder(string prefix, string attributeName)
        {
            return ObserverRegistration;
        }
    }
}
