using Neptuo.Templates.Compilation.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation
{
    public interface IContentBuilderRegistry
    {
        IComponentBuilder GetComponentBuilder(string prefix, string name);
        IComponentBuilder GetGenericContentBuilder(string name);
        ILiteralBuilder GetLiteralBuilder();
        IObserverRegistration GetObserverBuilder(string prefix, string name);

        bool ContainsComponent(string prefix, string name);
        bool ContainsObserver(string prefix, string name);

        void RegisterNamespace(NamespaceDeclaration namespaceDeclaration);
        IEnumerable<NamespaceDeclaration> GetRegisteredNamespaces();

        IContentBuilderRegistry CreateChildRegistry();
    }
}
