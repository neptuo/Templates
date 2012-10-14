using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Neptuo.Web.Framework.Utils
{
    public static class XmlExtensions
    {
        public static IEnumerable<XmlNode> ToEnumerable(this XmlNodeList list)
        {
            foreach (XmlNode node in list)
                yield return node;
        }
    }
}
