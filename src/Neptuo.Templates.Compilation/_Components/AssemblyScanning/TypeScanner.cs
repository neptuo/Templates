using Neptuo.Reflection;
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
        private readonly List<Action<Type>> processors = new List<Action<Type>>();
        private readonly Dictionary<string, HashSet<string>> assemblies = new Dictionary<string, HashSet<string>>();

        public TypeScanner()
            : this(new NullNameNormalizer())
        { }

        public TypeScanner(INameNormalizer typeNameNormalizer)
        {
            Guard.NotNull(typeNameNormalizer, "typeNameNormalizer");
            this.typeNameNormalizer = typeNameNormalizer;
        }

        public TypeScanner Add(string namespaceName, string assemblyFile)
        {
            Guard.NotNullOrEmpty(assemblyFile, "assemblyFile");

            if (String.IsNullOrEmpty(namespaceName))
                namespaceName = AllNamespaceWildcard;

            HashSet<string> namespaces;
            if (!assemblies.TryGetValue(assemblyFile, out namespaces))
                assemblies[assemblyFile] = namespaces = new HashSet<string>();

            namespaces.Add(namespaceName);
            return this;
        }

        public TypeScanner AddTypeFilter(Func<Type, bool> filter)
        {
            Guard.NotNull(filter, "filter");
            filters.Add(filter);
            return this;
        }

        public TypeScanner AddTypeProcessor(Action<Type> processor)
        {
            Guard.NotNull(processor, "processor");
            processors.Add(processor);
            return this;
        }

        /// <summary>
        /// Starts type scanning of all registered assemblies.
        /// </summary>
        public void Run()
        {
            IReflectionService service = ReflectionFactory.FromCurrentAppDomain();
            foreach (KeyValuePair<string, HashSet<string>> assemblyDescription in assemblies)
            {
                Assembly assembly = service.LoadAssembly(assemblyDescription.Key);
                RunAssembly(assembly, assemblyDescription.Value);
            }
        }

        private void RunAssembly(Assembly assembly, HashSet<string> namespaceNames)
        {
            foreach (Type type in assembly.GetTypes())
            {
                string typeNamespaceName = typeNameNormalizer.PrepareName(type.Namespace);
                if (IsNamespaceContained(namespaceNames, typeNamespaceName) && IsPassedThroughFilters(type))
                    ExecuteTypeProcessors(type);
            }
        }

        private bool IsNamespaceContained(HashSet<string> namespaceNames, string typeNamespaceName)
        {
            if (namespaceNames.Contains(typeNamespaceName))
                return true;

            foreach (string namespaceName in namespaceNames)
            {
                if (namespaceName.EndsWith(AllNamespaceWildcard))
                {
                    string subNamespaceName = namespaceName.Substring(0, namespaceName.Length - AllNamespaceWildcard.Length);
                    if (typeNamespaceName.StartsWith(subNamespaceName))
                        return true;
                }
            }

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

        private void ExecuteTypeProcessors(Type type)
        {
            foreach (Action<Type> processor in processors)
                processor(type);
        }
    }
}
