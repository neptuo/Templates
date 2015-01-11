using Neptuo;
using Neptuo.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleNG.SimpleContainer
{
    public class SimpleObjectBuilder : IDependencyContainer
    {
        private Dictionary<Type, Func<object>> registry;

        public SimpleObjectBuilder()
            : this(new Dictionary<Type, Func<object>>())
        { }

        public SimpleObjectBuilder(Dictionary<Type, Func<object>> registry)
        {
            this.registry = registry;
        }

        public IDependencyContainer RegisterInstance(Type t, string name, object instance)
        {
            registry[t] = () => instance;
            return this;
        }

        public IDependencyContainer RegisterType(Type from, Type to, string name, object lifetime)
        {
            registry[from] = () => Activator.CreateInstance(to);
            return this;
        }

        public IDependencyContainer CreateChildContainer()
        {
            return new SimpleObjectBuilder(new Dictionary<Type, Func<object>>(registry));
        }

        public object Resolve(Type t, string name)
        {
            if (registry.ContainsKey(t))
                return registry[t]();

            return Activator.CreateInstance(t);
        }

        public IEnumerable<object> ResolveAll(Type t)
        {
            throw new NotImplementedException();
        }
    }
}
