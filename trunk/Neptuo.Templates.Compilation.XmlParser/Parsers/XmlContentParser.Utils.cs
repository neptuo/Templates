using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Neptuo.Templates.Compilation.Parsers
{
    partial class XmlContentParser
    {
        public static class Utils
        {
            public static bool FindChildNode(XmlElement element, string name, out XmlNode result)
            {
                foreach (XmlNode node in element.ChildNodes)
                {
                    if (node.Name.ToLowerInvariant() == name)
                    {
                        result = node;
                        return true;
                    }
                }

                result = null;
                return false;
            }

            public static IEnumerable<XmlNode> FindNotUsedChildNodes(XmlNodeList childNodes, HashSet<string> usedNodes)
            {
                foreach (XmlNode node in childNodes)
                {
                    if (!usedNodes.Contains(node.Name.ToLowerInvariant()))
                        yield return node;
                }
            }

            public static bool NeedsServerProcessing(IContentBuilderRegistry builderRegistry, XmlElement element)
            {
                if (builderRegistry.ContainsComponent(element.Prefix, element.LocalName))
                    return true;

                foreach (XmlAttribute attribute in element.Attributes)
                {
                    //TODO: Reimplement! Creating unneeded builders!
                    if(builderRegistry.ContainsObserver(attribute.Prefix, attribute.LocalName))
                        return true;
                }

                return false;
            }

            public static IEnumerable<NamespaceDeclaration> GetXmlNsNamespace(XmlElement element)
            {
                List<NamespaceDeclaration> result = new List<NamespaceDeclaration>();
                foreach (XmlAttribute attribute in element.Attributes)
                {
                    if (attribute.Prefix.ToLowerInvariant() == "xmlns")
                        result.Add(new NamespaceDeclaration(attribute.LocalName, attribute.Value));
                }
                return result;
            }

            /// <summary>
            /// Creates child builder registry if needed (has any <paramref name="declarations"/>).
            /// </summary>
            /// <param name="currentBuilderRegistry">Current builder registry.</param>
            /// <param name="declarations">New namespace declaration.</param>
            /// <returns>Child builder registry or current one.</returns>
            public static IContentBuilderRegistry CreateChildRegistrator(IContentBuilderRegistry currentBuilderRegistry, IEnumerable<NamespaceDeclaration> declarations)
            {
                if (declarations.Any())
                {
                    IContentBuilderRegistry newBuilderRegistry = currentBuilderRegistry.CreateChildRegistry();
                    foreach (NamespaceDeclaration decl in declarations)
                        newBuilderRegistry.RegisterNamespace(decl);

                    return newBuilderRegistry;
                }
                return currentBuilderRegistry;
            }
        }
    }
}
