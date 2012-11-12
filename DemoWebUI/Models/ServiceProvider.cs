using Neptuo.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoWebUI.Models
{
    public class ServiceProvider : IServiceProvider
    {
        private IRegistrator registrator;
        private IComponentManager componentManager;

        public ServiceProvider(IRegistrator registrator, IComponentManager componentManager)
        {
            this.registrator = registrator;
            this.componentManager = componentManager;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(IRegistrator))
                return registrator;

            if (serviceType == typeof(IComponentManager))
                return componentManager;

            return null;
        }
    }
}