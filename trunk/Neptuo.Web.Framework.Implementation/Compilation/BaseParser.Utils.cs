using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Neptuo.Web.Framework.Compilation
{
    partial class BaseParser
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

            public static bool NeedsServerProcessing(XmlNode node)
            {
                XmlElement element = node as XmlElement;
                if (element == null)
                    return false;

                if (!String.IsNullOrWhiteSpace(element.Prefix))
                    return true;

                foreach (XmlAttribute attribute in element.Attributes)
                {
                    if (!String.IsNullOrWhiteSpace(attribute.Prefix))
                        return true;
                }

                return false;
            }
        }
    }
}
