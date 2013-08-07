using Neptuo.Templates.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class TypeScanner : TypeRegistryHelper
    {
        public TypeScanner(TypeBuilderRegistryConfiguration configuration, TypeBuilderRegistryContent content)
            : base(configuration, content)
        { }

        public void Scan(string prefix, string namespaceName)
        {
            prefix = PreparePrefix(prefix);

            List<Type> types = GetTypesInNamespace(namespaceName);
            RegisterControls(types, prefix);
        }

        protected void RegisterControls(List<Type> types, string prefix)
        {
            foreach (Type type in types)
            {
                if (CanBeUsedInMarkup(type, false))
                {
                    ComponentAttribute attribute = ReflectionHelper.GetAttribute<ComponentAttribute>(type);
                    string name = PrepareName(type.Name, Configuration.ComponentSuffix);
                    
                    if (attribute != null && !String.IsNullOrEmpty(attribute.Name))
                        name = PrepareName(attribute.Name, Configuration.ComponentSuffix);

                    Content.Components[prefix][name] = CreateFactory<IComponentBuilderFactory>(type, t => new DefaultControlBuilderFactory(t));
                }
            }
        }

        protected virtual T CreateFactory<T>(Type type, Func<Type, T> defaultFactory)
        {
            BuilderAttribute attribute = ReflectionHelper.GetAttribute<BuilderAttribute>(type);
            if (attribute != null)
            {
                foreach (Type builderType in attribute.Types)
                {
                    if (typeof(T).IsAssignableFrom(builderType))
                    {
                        T factory = (T)Configuration.DependencyProvider.Resolve(builderType, null);
                        return factory;
                    }
                }
            }
            return defaultFactory(type);
        }

        protected virtual List<Type> GetTypesInNamespace(string namespaceName)
        {
            Assembly targetAssembly = Assembly.GetExecutingAssembly();

            string[] parts = namespaceName.Split(',');
            if (parts.Length == 2)
                targetAssembly = Assembly.Load(parts[1]);

            return targetAssembly.GetTypes().Where(t => t.Namespace == parts[0]).ToList();
        }

        protected bool CanBeUsedInMarkup(Type type, bool requireDefaultCtor = true)
        {
            if (type.IsInterface)
                return false;

            if (type.IsAbstract)
                return false;

            if (requireDefaultCtor)
            {
                if (type.GetConstructor(new Type[] { }) == null)
                    return false;
            }

            return true;
        }

    }
}
