using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neptuo.Web.Html;
using Neptuo.Web.Html.Compilation;
using System.Reflection;

namespace Neptuo.Web.Mvc.ViewEngine.Utils
{
    public class ObjectActivator : IActivator
    {
        public ILivecycleObserver LivecycleObserver { get; set; }

        public IDependencyContainer DependencyContainer { get; set; }

        public ObjectActivator(IDependencyContainer container)
        {
            DependencyContainer = container;
            LivecycleObserver = container[typeof(ILivecycleObserver)] as ILivecycleObserver;
        }

        public object CreateInstance(Type type)
        {
            object instance = Activator.CreateInstance(type);
            ResolveDependencies(instance);
            LivecycleObserver.Add(instance);

            return instance;
        }

        private void ResolveDependencies(object instance)
        {
            if (instance == null)
                return;

            PropertyInfo[] properties = DependencyAttributes.GetProperties(instance.GetType());
            foreach (PropertyInfo prop in properties)
            {
                object result = DependencyContainer[prop.PropertyType];
                prop.SetValue(instance, result, null);
            }
        }
    }
}
