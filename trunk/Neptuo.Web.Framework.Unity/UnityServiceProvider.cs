using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Unity
{
    public class UnityServiceProvider : IExtendedServiceProvider, IServiceProvider
    {
        public IUnityContainer Container { get; private set; }

        public UnityServiceProvider(IUnityContainer container = null)
        {
            if (container == null)
                container = new UnityContainer();

            Container = container;
        }

        public object GetService(Type serviceType)
        {
            return Container.Resolve(serviceType);
        }

        public IExtendedServiceProvider CreateChildProvider()
        {
            return new UnityServiceProvider(Container.CreateChildContainer());
        }
    }
}
