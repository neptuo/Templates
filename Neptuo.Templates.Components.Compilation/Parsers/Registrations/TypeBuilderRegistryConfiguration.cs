using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class TypeBuilderRegistryConfiguration
    {
        public const string DefaultObserverWildcard = "*";
        public const string DefaultComponentSuffix = "control";
        public const string DefaultObserverSuffix = "observer";
        public const string DefaultExtensionSuffix = "extension";

        public bool CaseSensitive { get; private set; }
        public List<string> ComponentSuffix { get; private set; }
        public List<string> ObserverSuffix { get; private set; }
        public string ObserverWildcard { get; set; }
        public List<string> ExtensionSuffix { get; private set; }

        public IDependencyProvider DependencyProvider { get; private set; }
        public Type ExtensionType { get; private set; }

        public TypeBuilderRegistryConfiguration(IDependencyProvider dependencyProvider, Type extensionType)
        {
            Guard.NotNull(dependencyProvider, "dependencyProvider");
            Guard.NotNull(extensionType, "extensionType");
            DependencyProvider = dependencyProvider;
            ExtensionType = extensionType;
            ComponentSuffix = new List<string> { DefaultComponentSuffix };
            ObserverSuffix = new List<string> { DefaultObserverSuffix };
            ObserverWildcard = DefaultObserverWildcard;
            ExtensionSuffix = new List<string> { DefaultExtensionSuffix };
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
