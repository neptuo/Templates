using Microsoft.Practices.Unity;
using Neptuo.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestConsoleNG.Unity
{
    public class UnityDependencyContainer : IDependencyContainer, IDependencyProvider
    {
        public IUnityContainer Container { get; private set; }

        public UnityDependencyContainer(IUnityContainer container = null)
        {
            if (container == null)
                container = new UnityContainer();

            Container = container;
            Container.RegisterInstance<IDependencyProvider>(this);
        }

        public IDependencyContainer RegisterInstance(Type t, string name, object instance)
        {
            Container.RegisterInstance(t, name, instance);
            return this;
        }

        public IDependencyContainer RegisterType(Type from, Type to, string name)
        {
            Container.RegisterType(from, to, name);
            return this;
        }

        public IDependencyContainer CreateChildContainer()
        {
            return new UnityDependencyContainer(Container.CreateChildContainer());
        }

        public object Resolve(Type t, string name)
        {
            return Container.Resolve(t, name);
        }

        public IEnumerable<object> ResolveAll(Type t)
        {
            return Container.ResolveAll(t);
        }
    }
}
