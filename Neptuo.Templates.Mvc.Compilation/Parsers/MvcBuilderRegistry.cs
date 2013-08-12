using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class MvcBuilderRegistry : IContentBuilderRegistry
    {
        public IComponentBuilder GetComponentBuilder(string prefix, string name)
        {
            throw new NotImplementedException();
        }

        public IComponentBuilder GetGenericContentBuilder(string name)
        {
            throw new NotImplementedException();
        }

        public ILiteralBuilder GetLiteralBuilder()
        {
            throw new NotImplementedException();
        }

        public IObserverRegistration GetObserverBuilder(string prefix, string name)
        {
            throw new NotImplementedException();
        }

        public bool ContainsComponent(string prefix, string name)
        {
            throw new NotImplementedException();
        }

        public bool ContainsObserver(string prefix, string name)
        {
            return false;
        }

        public void RegisterNamespace(NamespaceDeclaration namespaceDeclaration)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<NamespaceDeclaration> GetRegisteredNamespaces()
        {
            throw new NotImplementedException();
        }

        public IContentBuilderRegistry CreateChildRegistry()
        {
            throw new NotImplementedException();
        }
    }
}
