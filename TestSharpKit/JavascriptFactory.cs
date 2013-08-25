using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Neptuo.Templates;

namespace MagicWare.ObjectBuilder
{
    public class DependencyAttribute : Attribute
    {
        // Zatím nefunguje - BUG, v dalším release bude opravený
        public string Name { get; set; }

        public DependencyAttribute()
        { }

        // Zatím nefunguje - BUG, v dalším release bude opravený
        public DependencyAttribute(string name)
        {
            Name = name;
        }
    }

    public class JavascriptFactory : IDependencyProvider, IDependencyContainer
    {
        public JavascriptFactory()
        { }

        public void RegisterType(Type source, Type target, string name)
        {
        }

        public object Resolve(Type type, string name = null)
        {
            return null;
        }

        public IEnumerable<object> ResolveAll(Type type)
        {
            return null;
        }

        public object BuildUp(Type type, object existing, string name)
        {
            return null;
        }


        public IDependencyContainer CreateChildContainer()
        {
            throw new NotImplementedException();
        }

        public IDependencyContainer RegisterInstance(Type t, string name, object instance)
        {
            throw new NotImplementedException();
        }

        IDependencyContainer IDependencyContainer.RegisterType(Type from, Type to, string name)
        {
            throw new NotImplementedException();
        }
    }

}
