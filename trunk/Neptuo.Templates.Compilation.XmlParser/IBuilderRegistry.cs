using Neptuo.Templates.Compilation.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation
{
    public interface IBuilderRegistry
    {
        IComponentBuilder GetComponentBuilder(string prefix, string name);
        IObserverBuilder GetObserverBuilder(string prefix, string name);

        void RegisterNamespace(NamespaceDeclaration namespaceDeclaration);
        IEnumerable<NamespaceDeclaration> GetRegisteredNamespaces();

        IBuilderRegistry CreateChildRegistry();
    }
}
