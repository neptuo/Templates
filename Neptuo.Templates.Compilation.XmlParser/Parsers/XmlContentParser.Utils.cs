using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers
{
    partial class XmlContentParser
    {
        public static class Utils
        {
            public static bool FindChildNode(IXmlElement sourceElement, string name, out IXmlNode result)
            {
                foreach (IXmlNode node in sourceElement.ChildNodes)
                {
                    if (node.NodeType == XmlNodeType.Element)
                    {
                        IXmlElement element = (IXmlElement)node;
                        if (element.Name.ToLowerInvariant() == name)
                        {
                            result = node;
                            return true;
                        }
                    }
                }

                result = null;
                return false;
            }

            public static IEnumerable<IXmlNode> FindNotUsedChildNodes(IEnumerable<IXmlNode> childNodes, HashSet<string> usedNodes)
            {
                foreach (IXmlNode node in childNodes)
                {
                    if (node.NodeType == XmlNodeType.Element)
                    {
                        IXmlElement element = (IXmlElement)node;
                        if (!usedNodes.Contains(element.Name.ToLowerInvariant()))
                            yield return node;
                    }
                    else
                    {
                        yield return node;
                    }
                }
            }
            
            public static bool NeedsServerProcessing(Helper helper, IContentBuilderRegistry builderRegistry, IXmlElement element)
            {
                if (builderRegistry.ContainsComponent(element.Prefix, element.LocalName))
                    return true;

                foreach (IXmlAttribute attribute in element.Attributes)
                {
                    if(builderRegistry.ContainsObserver(attribute.Prefix, attribute.LocalName))
                        return true;

                    if (attribute.Value.StartsWith("{"))
                        return true; //TODO: Fix for {Expressions}
                }

                return false;
            }

            public static IEnumerable<NamespaceDeclaration> GetXmlNsNamespace(IXmlElement element)
            {
                List<NamespaceDeclaration> result = new List<NamespaceDeclaration>();
                foreach (IXmlAttribute attribute in element.Attributes)
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
