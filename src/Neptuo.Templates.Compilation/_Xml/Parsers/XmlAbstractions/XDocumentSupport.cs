using Neptuo.ComponentModel;
using Neptuo.ComponentModel.TextOffsets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using SystemXmlNodeType = System.Xml.XmlNodeType;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// XML abstraction for System.Xml.Linq
    /// </summary>
    internal class XDocumentSupport
    {
        private class FakeXmlNamespaceManager : XmlNamespaceManager
        {

            public FakeXmlNamespaceManager()
                : base(new NameTable())
            { }

            public override string LookupNamespace(string prefix)
            {
                return "template-" + prefix;
            }
        }

        public static XElementWrapper LoadXml(string xmlContent)
        {
            //XmlNamespaceManager namespaceManager = new FakeXmlNamespaceManager();
            //XmlParserContext parserContext = new XmlParserContext(null, namespaceManager, null, XmlSpace.Preserve);
            XmlReaderSettings readerSettings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit, ValidationFlags = XmlSchemaValidationFlags.AllowXmlAttributes };
            using (XmlReader reader = XmlReader.Create(new StringReader(xmlContent), readerSettings))
            {
                XDocument document = XDocument.Load(reader, LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo);
                return new XElementWrapper(document.Root);
            }
        }

        internal abstract class XNodeWrapper : IXmlNode, ILineInfo
        {
            public XmlNodeType NodeType { get; private set; }
            public abstract string OuterXml { get; }

            #region ILineInfo

            public int LineIndex { get; protected set; }
            public int ColumnIndex { get; private set; }

            #endregion

            public XNodeWrapper(XmlNodeType nodeType)
            {
                NodeType = nodeType;
            }

            protected void SetLineInfo(IXmlLineInfo lineInfo)
            {
                if (lineInfo.HasLineInfo())
                {
                    LineIndex = lineInfo.LineNumber;
                    ColumnIndex = lineInfo.LinePosition;
                }
            }

            public override string ToString()
            {
                return OuterXml;
            }
        }

        internal abstract class XNameNodeWrapper : XNodeWrapper, IXmlName
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

            public XNameNodeWrapper(XmlNodeType nodeType, string prefix, string localName)
                : base(nodeType)
            {
                Prefix = prefix ?? String.Empty;
                LocalName = localName;
            }
        }

        internal class XElementWrapper : XNameNodeWrapper, IXmlElement
        {
            public IEnumerable<IXmlNode> ChildNodes { get; private set; }
            public IEnumerable<IXmlAttribute> Attributes { get; private set; }
            public bool IsEmpty { get; private set; }
            public override string OuterXml { get { throw Ensure.Exception.NotImplemented(); } }

            public XElementWrapper(XElement element)
                : base(XmlNodeType.Element, element.GetPrefixOfNamespace(element.Name.Namespace), element.Name.LocalName)
            {
                ChildNodes = CreateChildNodes(element.Nodes());
                Attributes = CreateAttributes(element.Attributes(), element, this);
                IsEmpty = element.IsEmpty;
                SetLineInfo(element);
            }

            public override string ToString()
            {
                return String.Format(
                    "<{0} {1}{2}",
                    Name,
                    String.Join(" ", Attributes.OfType<XAttributeWrapper>().Select(a => a.OuterXml)),
                    ChildNodes.Any() ? ">..." : " />"
                );
            }

            public static IEnumerable<IXmlNode> CreateChildNodes(IEnumerable<XNode> childNodes)
            {
                List<IXmlNode> result = new List<IXmlNode>();
                foreach (XNode node in childNodes)
                {
                    if (node.NodeType == SystemXmlNodeType.Element)
                        result.Add(new XElementWrapper((XElement)node));
                    else if (node.NodeType == SystemXmlNodeType.Text)
                        result.Add(new XTextWrapper((XText)node));
                    else if (node.NodeType == SystemXmlNodeType.Whitespace)
                        result.Add(new XTextWrapper((XText)node));
                    else if (node.NodeType == SystemXmlNodeType.SignificantWhitespace)
                        result.Add(new XTextWrapper((XText)node));
                    else if (node.NodeType == SystemXmlNodeType.Comment)
                        result.Add(new XCommentWrapper((XComment)node));
                }
                return result;
            }

            public static IEnumerable<IXmlAttribute> CreateAttributes(IEnumerable<XAttribute> attributes, XElement sourceElement, XElementWrapper ownerElement)
            {
                List<IXmlAttribute> result = new List<IXmlAttribute>();
                foreach (XAttribute attribute in attributes)
                    result.Add(new XAttributeWrapper(attribute, sourceElement.GetPrefixOfNamespace(attribute.Name.Namespace), ownerElement));

                return result;
            }
        }

        internal class XAttributeWrapper : XNameNodeWrapper, IXmlAttribute
        {
            public string Value { get; private set; }
            public IXmlElement OwnerElement { get; private set; }
            public override string OuterXml { get { return String.Format("{0}=\"{1}\"", Name, Value); } }

            public XAttributeWrapper(XAttribute attribute, string prefix, IXmlElement ownerElement)
                : base(XmlNodeType.Text, prefix, attribute.Name.LocalName)
            {
                Value = attribute.Value;
                OwnerElement = ownerElement;
                SetLineInfo(attribute);
            }
        }

        internal class XTextWrapper : XNodeWrapper, IXmlText, ILineInfo
        {
            public string Text { get; private set; }
            public override string OuterXml { get { return Text; } }

            public XTextWrapper(string text)
                : base(XmlNodeType.Text)
            {
                Text = text;
            }

            public XTextWrapper(XText text)
                : this(text.Value)
            {
                SetLineInfo(text);
            }

            //public XmlTextWrapper(XmlWhitespace text)
            //    : this(text.InnerText)
            //{ }

            //public XmlTextWrapper(XmlSignificantWhitespace text)
            //    : this(text.InnerText)
            //{ }
        }

        internal class XCommentWrapper : XNodeWrapper, IXmlText, ILineInfo
        {
            public string Text { get; private set; }
            public override string OuterXml { get { return String.Format("<!-- {0} -->", Text); } }

            public XCommentWrapper(XComment comment)
                : base(XmlNodeType.Comment)
            {
                Text = comment.Value;
                SetLineInfo(comment);
            }
        }
    }
}
