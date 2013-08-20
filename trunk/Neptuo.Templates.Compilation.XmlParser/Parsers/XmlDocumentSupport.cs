using Neptuo.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XXmlDocument = System.Xml.XmlDocument;
using XXmlNodeList = System.Xml.XmlNodeList;
using XXmlNode = System.Xml.XmlNode;
using XXmlNodeType = System.Xml.XmlNodeType;
using XXmlElement = System.Xml.XmlElement;
using XXmlAttributeCollection = System.Xml.XmlAttributeCollection;
using XXmlAttribute = System.Xml.XmlAttribute;
using XXmlText = System.Xml.XmlText;
using XXmlWhitespace = System.Xml.XmlWhitespace;
using XXmlSignificantWhitespace = System.Xml.XmlSignificantWhitespace;
using XXmlComment = System.Xml.XmlComment;

namespace Neptuo.Templates.Compilation.Parsers
{
    internal class XmlDocumentSupport
    {
        public static XmlElement LoadXml(string xmlContent)
        {
            XXmlDocument document = new XXmlDocument();
            document.PreserveWhitespace = true;
            document.LoadXml(xmlContent);
            return new XmlDocumentSupport.XmlElement(document.DocumentElement);
        }

        internal abstract class XmlNode : IXmlNode
        {
            public XmlNodeType NodeType { get; private set; }
            public abstract string OuterXml { get; }

            public XmlNode(XmlNodeType nodeType)
            {
                NodeType = nodeType;
            }
        }

        internal abstract class XmlNameNode : XmlNode, IXmlName
        {
            internal const char NameSeparator = ':';

            public string Name
            {
                get
                {
                    if (String.IsNullOrEmpty(Prefix))
                        return LocalName;

                    return Prefix + NameSeparator + LocalName;
                }
            }
            public string Prefix { get; private set; }
            public string LocalName { get; private set; }

            public XmlNameNode(XmlNodeType nodeType, string name)
                : base(nodeType)
            {
                if (String.IsNullOrEmpty(name))
                    throw new ArgumentNullException("name");

                string[] parts = name.Split(NameSeparator);
                if (parts.Length > 2)
                    throw new ArgumentOutOfRangeException("name", "Name must in format 'prefix:name' or 'name'!");

                if (parts.Length == 1)
                {
                    Prefix = String.Empty;
                    LocalName = parts[0];
                }
                else
                {
                    Prefix = parts[0];
                    LocalName = parts[1];
                }
            }
        }

        internal class XmlElement : XmlNameNode, IXmlElement
        {
            private string outerXml;

            public IEnumerable<IXmlNode> ChildNodes { get; private set; }
            public IEnumerable<IXmlAttribute> Attributes { get; private set; }
            public bool IsEmpty { get; private set; }
            public override string OuterXml { get { return outerXml; } }

            public XmlElement(XXmlElement element)
                : base(XmlNodeType.Element, element.Name)
            {
                ChildNodes = CreateChildNodes(element.ChildNodes);
                Attributes = CreateAttributes(element.Attributes, this);
                IsEmpty = element.IsEmpty;
                outerXml = element.OuterXml;
            }

            public static IEnumerable<IXmlNode> CreateChildNodes(XXmlNodeList childNodes)
            {
                List<IXmlNode> result = new List<IXmlNode>();
                foreach (XXmlNode node in childNodes)
                {
                    if (node.NodeType == XXmlNodeType.Element)
                        result.Add(new XmlElement((XXmlElement)node));
                    else if (node.NodeType == XXmlNodeType.Text)
                        result.Add(new XmlText((XXmlText)node));
                    else if (node.NodeType == XXmlNodeType.Whitespace)
                        result.Add(new XmlText((XXmlWhitespace)node));
                    else if (node.NodeType == XXmlNodeType.SignificantWhitespace)
                        result.Add(new XmlText((XXmlSignificantWhitespace)node));
                    else if (node.NodeType == XXmlNodeType.Comment)
                        result.Add(new XmlComment((XXmlComment)node));
                }
                return result;
            }

            public static IEnumerable<IXmlAttribute> CreateAttributes(XXmlAttributeCollection attributes, XmlElement ownerElement)
            {
                List<IXmlAttribute> result = new List<IXmlAttribute>();
                foreach (XXmlAttribute attribute in attributes)
                    result.Add(new XmlAttribute(attribute, ownerElement));

                return result;
            }
        }

        internal class XmlAttribute : XmlNameNode, IXmlAttribute
        {
            public string Value { get; private set; }
            public IXmlElement OwnerElement { get; private set; }
            public override string OuterXml { get { return String.Format("{0}=\"{1}\"", Name, Value); } }

            public XmlAttribute(XXmlAttribute attribute, IXmlElement ownerElement)
                : base(XmlNodeType.Text, attribute.Name)
            {
                Value = attribute.Value;
                OwnerElement = ownerElement;
            }
        }

        internal class XmlText : XmlNode, IXmlText
        {
            public string Text { get; private set; }
            public override string OuterXml { get { return Text; } }

            public XmlText(string text)
                : base(XmlNodeType.Text)
            {
                Text = text;
            }

            public XmlText(XXmlText text)
                : this(text.InnerText)
            { }

            public XmlText(XXmlWhitespace text)
                : this(text.InnerText)
            { }

            public XmlText(XXmlSignificantWhitespace text)
                : this(text.InnerText)
            { }
        }

        internal class XmlComment : XmlNode, IXmlText
        {
            public string Text { get; private set; }
            public override string OuterXml { get { return Text; } }

            public XmlComment(XXmlComment comment)
                : base(XmlNodeType.Comment)
            {
                Text = comment.InnerText;
            }
        }

    }
}
