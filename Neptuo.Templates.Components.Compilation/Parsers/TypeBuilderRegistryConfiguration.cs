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
        public string ComponentSuffix { get; private set; }
        public string ObserverSuffix { get; private set; }
        public string ObserverWildcard { get; set; }
        public string ExtensionSuffix { get; private set; }

        public IDependencyProvider DependencyProvider { get; private set; }

        public TypeBuilderRegistryConfiguration(IDependencyProvider dependencyProvider)
        {
            DependencyProvider = dependencyProvider;
            ComponentSuffix = DefaultComponentSuffix;
            ObserverSuffix = DefaultObserverSuffix;
            ObserverWildcard = DefaultObserverWildcard;
            ExtensionSuffix = DefaultExtensionSuffix;
        }
    }
}
