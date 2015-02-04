using Neptuo.Reflection;
using Neptuo.Templates.Compilation.Parsers;
using Neptuo.Templates.Compilation.Parsers.Normalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.AssemblyScanning
{
    public class TypeScanner
    {
        public const string AllNamespaceWildcard = "*";

        private readonly INameNormalizer typeNameNormalizer;
        private readonly List<Func<Type, bool>> filters = new List<Func<Type, bool>>();
        private readonly List<Action<string, Type>> processors = new List<Action<string, Type>>();
        private readonly Dictionary<string, List<NamespaceItem>> assemblies = new Dictionary<string, List<NamespaceItem>>();

        public TypeScanner()
            : this(new NullNameNormalizer())
        { }

        public TypeScanner(INameNormalizer typeNameNormalizer)
        {
            Guard.NotNull(typeNameNormalizer, "typeNameNormalizer");
            this.typeNameNormalizer = typeNameNormalizer;
        }

        public TypeScanner AddAssembly(string prefix, string namespaceName, string assemblyFile)
        {
            Guard.NotNullOrEmpty(assemblyFile, "assemblyFile");

            if (String.IsNullOrEmpty(namespaceName))
                namespaceName = AllNamespaceWildcard;

            List<NamespaceItem> namespaces;
            if (!assemblies.TryGetValue(assemblyFile, out namespaces))
                assemblies[assemblyFile] = namespaces = new List<NamespaceItem>();

            namespaces.Add(new NamespaceItem(prefix, namespaceName));
            return this;
        }

        public TypeScanner AddTypeFilter(Func<Type, bool> filter)
        {
            Guard.NotNull(filter, "filter");
            filters.Add(filter);
            return this;
        }

        public TypeScanner AddTypeProcessor(Action<string, Type> processor)
        {
            Guard.NotNull(processor, "processor");
            processors.Add(processor);
            return this;
        }

        public IEnumerable<NamespaceDeclaration> EnumerateUsedNamespaces()
        {
            List<NamespaceDeclaration> result = new List<NamespaceDeclaration>();
            foreach (KeyValuePair<string, List<NamespaceItem>> item in assemblies)
                result.AddRange(item.Value.Select(n => new NamespaceDeclaration(n.Prefix, n.NamespaceName, item.Key)));

            return result;
        }

        /// <summary>
        /// Starts type scanning of all registered assemblies.
        /// </summary>
        public void Run()
        {
            IReflectionService service = ReflectionFactory.FromCurrentAppDomain();
            foreach (KeyValuePair<string, List<NamespaceItem>> assemblyDescription in assemblies)
            {
                Assembly assembly = service.LoadAssembly(assemblyDescription.Key);
                RunAssembly(assembly, assemblyDescription.Value);
            }
        }

        private void RunAssembly(Assembly assembly, List<NamespaceItem> namespaceNames)
        {
            foreach (Type type in assembly.GetTypes())
            {
                string typeNamespaceName = typeNameNormalizer.PrepareName(type.Namespace);
                string prefix;
                if (TryGetNamespacePrefix(namespaceNames, typeNamespaceName, out prefix) && IsPassedThroughFilters(type))
                    ExecuteTypeProcessors(prefix, type);
            }
        }

        private bool TryGetNamespacePrefix(List<NamespaceItem> namespaceNames, string typeNamespaceName, out string prefix)
        {
            foreach (NamespaceItem namespaceItem in namespaceNames)
            {
                if (namespaceItem.NamespaceName == typeNamespaceName)
                {
                    prefix = namespaceItem.Prefix;
                    return true;
                }

                if (namespaceItem.NamespaceName.EndsWith(AllNamespaceWildcard))
                {
                    string subNamespaceName = namespaceItem.NamespaceName.Substring(0, namespaceItem.NamespaceName.Length - AllNamespaceWildcard.Length);
                    if (typeNamespaceName.StartsWith(subNamespaceName))
                    {
                        prefix = namespaceItem.Prefix;
                        return true;
                    }
                }
            }

            prefix = null;
            return false;
        }

        private bool IsPassedThroughFilters(Type type)
        {
            foreach (Func<Type, bool> filter in filters)
            {
                if (!filter(type))
                    return false;
            }

            return true;
        }

        private void ExecuteTypeProcessors(string prefix, Type type)
        {
            foreach (Action<string, Type> processor in processors)
                processor(prefix, type);
        }

        private class NamespaceItem
        {
            public string Prefix { get; private set; }
            public string NamespaceName { get; private set; }

            public NamespaceItem(string prefix, string namespaceName)
            {
                Prefix = prefix;
                NamespaceName = namespaceName;
            }
        }
    }
}
