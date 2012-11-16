using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Neptuo.Web.Framework.Compilation.Parsers
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

            public static IEnumerable<XmlNode> FindNotUsedChildNodes(XmlElement element, HashSet<string> usedNodes)
            {
                foreach (XmlNode node in element.ChildNodes)
                {
                    if (!usedNodes.Contains(node.Name.ToLowerInvariant()))
                        yield return node;
                }
            }

            public static bool NeedsServerProcessing(IRegistrator registrator, XmlElement element)
            {
                if (registrator.GetControl(element.Prefix, element.LocalName) != null)
                    return true;

                foreach (XmlAttribute attribute in element.Attributes)
                {
                    if(registrator.GetObserver(attribute.Prefix, attribute.LocalName) != null)
                        return true;
                }

                return false;
            }

            public static IEnumerable<NamespaceDeclaration> GetXmlNsClrNamespace(XmlElement element)
            {
                List<NamespaceDeclaration> result = new List<NamespaceDeclaration>();
                foreach (XmlAttribute attribute in element.Attributes)
                {
                    if (attribute.Prefix.ToLowerInvariant() == "xmlns")
                    {
                        if (attribute.Value.StartsWith("clr-namespace:"))
                            result.Add(new NamespaceDeclaration(attribute.LocalName, attribute.Value.Substring("clr-namespace:".Length)));
                    }
                }
                return result;
            }

            public static IRegistrator CreateChildRegistrator(IRegistrator currentRegistrator, IEnumerable<NamespaceDeclaration> declarations)
            {
                IRegistrator newRegistrator = currentRegistrator.CreateChildRegistrator();
                if (declarations.Any())
                {
                    foreach (NamespaceDeclaration decl in declarations)
                        newRegistrator.RegisterNamespace(decl.Prefix, decl.Namespace);
                }
                return newRegistrator;
            }
        }
    }
}
