﻿using Neptuo.Reflection;
using Neptuo.Templates.Controls;
using Neptuo.Templates.Extensions;
using Neptuo.Templates.Observers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Helper for scanning namespaces for types which can be registered.
    /// </summary>
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
            RegisterTokens(types, prefix);
        }

        protected void RegisterControls(List<Type> types, string prefix)
        {
            foreach (Type type in types)
            {
                if (CanBeUsedInMarkup(type, false))
                {
                    string name = GetComponentName(type, Configuration.ComponentSuffix);
                    Content.Components[prefix][name] = CreateFactory<IContentBuilderFactory>(type, CreateDefaultComponentBuilderFactory);
                }
            }
        }

        protected void RegisterTokens(List<Type> types, string prefix)
        {
            foreach (Type type in types)
            {
                if (CanBeUsedInMarkup(type, false) && ImplementsInterface<IValueExtension>(type))
                {
                    string name = GetComponentName(type, Configuration.ExtensionSuffix);
                    Content.Tokens[prefix][name] = CreateFactory<ITokenBuilderFactory>(type, CreateDefaultTokenBuilderFactory);
                }
            }
        }

        protected virtual string GetComponentName(Type type, IEnumerable<string> suffix)
        {
            ComponentAttribute attribute = ReflectionHelper.GetAttribute<ComponentAttribute>(type);
            string name = PrepareName(type.Name, suffix);

            if (attribute != null && !String.IsNullOrEmpty(attribute.Name))
                name = PrepareName(attribute.Name, suffix);

            return name;
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

        protected virtual IContentBuilderFactory CreateDefaultComponentBuilderFactory(Type type)
        {
            return new DefaultTypeComponentBuilderFactory(type);
        }

        protected virtual ITokenBuilderFactory CreateDefaultTokenBuilderFactory(Type type)
        {
            return new DefaultTokenBuilderFactory(type);
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

        protected bool ImplementsInterface<T>(Type type)
        {
            return typeof(T).IsAssignableFrom(type);
        }
    }
}
