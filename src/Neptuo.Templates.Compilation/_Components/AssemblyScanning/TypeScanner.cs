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
        /// <summary>
        /// Any sub namespace character.
        /// </summary>
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

        /// <summary>
        /// Registers types from <paramref name="assemblyFile"/> (with namespace <paramref name="namespaceName"/>) to prefix <paramref name="prefix"/>.
        /// Parameter<paramref name="namespaceName"/> can end with '*' to instruct this component to include namespace and all sub namespaces.
        /// If <paramref name="namespaceName"/> is only '*', it means 'all types from <paramref name="assemblyFile"/>'.
        /// </summary>
        /// <param name="prefix">Prefix to register types with.</param>
        /// <param name="namespaceName">Required type namespace.</param>
        /// <param name="assemblyFile">Assembly to scan types from.</param>
        /// <returns>Self (fluently).</returns>
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

        /// <summary>
        /// Registers delegate to filter out scanned types.
        /// Only when all filters for a particular type returns <c>true</c>,
        /// the type is promoted for processing by processors.
        /// </summary>
        /// <param name="filter">Delegate to filter out types.</param>
        /// <returns>Self (fluently).</returns>
        public TypeScanner AddTypeFilter(Func<Type, bool> filter)
        {
            Guard.NotNull(filter, "filter");
            filters.Add(filter);
            return this;
        }

        /// <summary>
        /// Registers delegate to process (register) scanned types.
        /// </summary>
        /// <param name="processor">Delegate to process scanned types.</param>
        /// <returns>Self (fluently).</returns>
        public TypeScanner AddTypeProcessor(Action<string, Type> processor)
        {
            Guard.NotNull(processor, "processor");
            processors.Add(processor);
            return this;
        }

        /// <summary>
        /// Enumerates all scanned namespaces.
        /// </summary>
        /// <returns>Enumeration of all scanned namespaces.</returns>
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
                //Assembly assembly = service.LoadAssembly(assemblyDescription.Key + ".dll");
                Assembly assembly = Assembly.Load(assemblyDescription.Key);
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
                    if (subNamespaceName.EndsWith("."))
                        subNamespaceName = subNamespaceName.Substring(0, subNamespaceName.Length - 1);

                    if (String.IsNullOrEmpty(subNamespaceName) || typeNamespaceName.StartsWith(subNamespaceName))
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
