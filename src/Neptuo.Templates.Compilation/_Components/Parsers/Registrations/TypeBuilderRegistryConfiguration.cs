using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class TypeBuilderRegistryConfiguration
    {
        private IDependencyProvider dependencyProvider;

        public const string DefaultObserverWildcard = "*";
        public const string DefaultComponentSuffix = "control";
        public const string DefaultObserverSuffix = "observer";
        public const string DefaultExtensionSuffix = "extension";

        public bool CaseSensitive { get; private set; }
        public List<string> ComponentSuffix { get; private set; }
        public List<string> ObserverSuffix { get; private set; }
        public string ObserverWildcard { get; set; }
        public List<string> ExtensionSuffix { get; private set; }

        public IDependencyProvider DependencyProvider
        {
            get
            {
                if (dependencyProvider == null)
                    throw Guard.Exception.InvalidOperation("When constructing TypeBuilderConfiguration without dependency provider, that it's not possible to use it.");

                return dependencyProvider;
            }
            set { dependencyProvider = value; }
        }

        public TypeBuilderRegistryConfiguration()
        {
            ComponentSuffix = new List<string> { DefaultComponentSuffix };
            ObserverSuffix = new List<string> { DefaultObserverSuffix };
            ObserverWildcard = DefaultObserverWildcard;
            ExtensionSuffix = new List<string> { DefaultExtensionSuffix };
        }

        public TypeBuilderRegistryConfiguration(IDependencyProvider dependencyProvider)
            : this()
        {
            Guard.NotNull(dependencyProvider, "dependencyProvider");
            DependencyProvider = dependencyProvider;
        }


        public TypeBuilderRegistryConfiguration AddComponentSuffix(string suffix)
        {
            ComponentSuffix.Add(suffix);
            return this;
        }

        public TypeBuilderRegistryConfiguration AddObserverSuffix(string suffix)
        {
            ObserverSuffix.Add(suffix);
            return this;
        }

        public TypeBuilderRegistryConfiguration AddExtensionSuffix(string suffix)
        {
            ExtensionSuffix.Add(suffix);
            return this;
        }
    }
}
