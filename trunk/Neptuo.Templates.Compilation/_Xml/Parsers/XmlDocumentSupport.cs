using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SystemXmlNodeType = System.Xml.XmlNodeType;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// XML abstraction for System.Xml.
    /// </summary>
    internal class XmlDocumentSupport
    {
        public static XmlElementWrapper LoadXml(string xmlContent)
        {
            XmlDocument document = new XmlDocument();
            document.PreserveWhitespace = true;
            document.LoadXml(xmlContent);
            return new XmlDocumentSupport.XmlElementWrapper(document.DocumentElement);
        }

        internal abstract class XmlNodeWrapper : IXmlNode
        {
            public XmlNodeType NodeType { get; private set; }
            public abstract string OuterXml { get; }

            public XmlNodeWrapper(XmlNodeType nodeType)
            {
                NodeType = nodeType;
            }
        }

        internal abstract class XmlNameNodeWrapper : XmlNodeWrapper, IXmlName
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

            public XmlNameNodeWrapper(XmlNodeType nodeType, string name)
                : base(nodeType)
            {
                Guard.NotNullOrEmpty(name, "name");

                string[] parts = name.Split(NameSeparator);
                if (parts.Length > 2)
                    throw Guard.Exception.ArgumentOutOfRange("name", "Name must in format 'prefix:name' or 'name'!");

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

        internal class XmlElementWrapper : XmlNameNodeWrapper, IXmlElement
        {
            private string outerXml;

            public IEnumerable<IXmlNode> ChildNodes { get; private set; }
            public IEnumerable<IXmlAttribute> Attributes { get; private set; }
            public bool IsEmpty { get; private set; }
            public override string OuterXml { get { return outerXml; } }

            public XmlElementWrapper(XmlElement element)
                : base(XmlNodeType.Element, element.Name)
            {
                ChildNodes = CreateChildNodes(element.ChildNodes);
                Attributes = CreateAttributes(element.Attributes, this);
                IsEmpty = element.IsEmpty;
                outerXml = element.OuterXml;
            }

            public static IEnumerable<IXmlNode> CreateChildNodes(XmlNodeList childNodes)
            {
                List<IXmlNode> result = new List<IXmlNode>();
                foreach (XmlNode node in childNodes)
                {
                    if (node.NodeType == SystemXmlNodeType.Element)
                        result.Add(new XmlElementWrapper((XmlElement)node));
                    else if (node.NodeType == SystemXmlNodeType.Text)
                        result.Add(new XmlTextWrapper((XmlText)node));
                    else if (node.NodeType == SystemXmlNodeType.Whitespace)
                        result.Add(new XmlTextWrapper((XmlWhitespace)node));
                    else if (node.NodeType == SystemXmlNodeType.SignificantWhitespace)
                        result.Add(new XmlTextWrapper((XmlSignificantWhitespace)node));
                    else if (node.NodeType == SystemXmlNodeType.Comment)
                        result.Add(new XmlCommentWrapper((XmlComment)node));
                }
                return result;
            }

            public static IEnumerable<IXmlAttribute> CreateAttributes(XmlAttributeCollection attributes, XmlElementWrapper ownerElement)
            {
                List<IXmlAttribute> result = new List<IXmlAttribute>();
                foreach (XmlAttribute attribute in attributes)
                    result.Add(new XmlAttributeWrapper(attribute, ownerElement));

                return result;
            }
        }

        internal class XmlAttributeWrapper : XmlNameNodeWrapper, IXmlAttribute
        {
            public string Value { get; private set; }
            public IXmlElement OwnerElement { get; private set; }
            public override string OuterXml { get { return String.Format("{0}=\"{1}\"", Name, Value); } }

            public XmlAttributeWrapper(XmlAttribute attribute, IXmlElement ownerElement)
                : base(XmlNodeType.Text, attribute.Name)
            {
                Value = attribute.Value;
                OwnerElement = ownerElement;
            }
        }

        internal class XmlTextWrapper : XmlNodeWrapper, IXmlText
        {
            public string Text { get; private set; }
            public override string OuterXml { get { return Text; } }

            public XmlTextWrapper(string text)
                : base(XmlNodeType.Text)
            {
                Text = text;
            }

            public XmlTextWrapper(XmlText text)
                : this(text.InnerText)
            { }

            public XmlTextWrapper(XmlWhitespace text)
                : this(text.InnerText)
            { }

            public XmlTextWrapper(XmlSignificantWhitespace text)
                : this(text.InnerText)
            { }
        }

        internal class XmlCommentWrapper : XmlNodeWrapper, IXmlText
        {
            public string Text { get; private set; }
            public override string OuterXml { get { return Text; } }

            public XmlCommentWrapper(XmlComment comment)
                : base(XmlNodeType.Comment)
            {
                Text = comment.InnerText;
            }
        }

    }
}
