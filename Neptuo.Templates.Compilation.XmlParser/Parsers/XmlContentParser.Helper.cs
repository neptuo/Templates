using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Neptuo.Templates.Compilation.CodeObjects;

namespace Neptuo.Templates.Compilation.Parsers
{
    partial class XmlContentParser
    {
        public class Helper
        {
            public IPropertyDescriptor Parent { get; set; }
            public IContentParserContext Context { get; protected set; }
            public XmlDocument Document { get; protected set; }
            public IBuilderRegistry BuilderRegistry { get; set; }
            public StringBuilder Content { get; protected set; }

            public Helper(string xml, IContentParserContext context, IBuilderRegistry builderRegistry)
            {
                Context = context;
                BuilderRegistry = builderRegistry;
                Content = new StringBuilder();
                Parent = context.PropertyDescriptor;

                if (xml != null)
                {
                    Document = new XmlDocument();
                    Document.LoadXml(CreateRootElement(xml));
                }
            }

            public void WithParent(IPropertyDescriptor parent, Action execute)
            {
                IPropertyDescriptor current = Parent;
                Parent = parent;
                execute();
                Parent = current;
            }

            public void FormatEmptyElement(XmlElement element)
            {
                Content.AppendFormat("<{0}", element.Name);
                foreach (XmlAttribute attribute in element.Attributes)
                    Content.AppendFormat(" {0}=\"{1}\"", attribute.Name, attribute.Value);

                Content.Append(" />");
            }

            public void FormatStartElement(XmlElement element)
            {
                Content.AppendFormat("<{0}", element.Name);
                foreach (XmlAttribute attribute in element.Attributes)
                    Content.AppendFormat(" {0}=\"{1}\"", attribute.Name, attribute.Value);

                Content.Append(">");
            }

            public void FormatEndElement(XmlElement element)
            {
                Content.AppendFormat("</{0}>", element.Name);
            }

            private string CreateRootElement(string content)
            {
                HashSet<string> usedPrefixes = new HashSet<string>();
                StringBuilder result = new StringBuilder();

                result.Append("<?xml version=\"1.0\" ?>");
                result.Append("<Neptuo-Templates-Root");
                foreach (NamespaceDeclaration entry in BuilderRegistry.GetRegisteredNamespaces())
                {
                    if (usedPrefixes.Add(entry.Prefix))
                    {
                        result.AppendFormat(
                            " xmlns{0}=\"clr-namespace:{1}\"", 
                            String.IsNullOrEmpty(entry.Prefix) ? String.Empty : String.Format(":{0}", entry.Prefix), 
                            entry.Namespace
                        );
                    }
                }
                result.Append(">");
                result.Append(content);
                result.Append("</Neptuo-Templates-Root>");

                return result.ToString();
            }
        }
    }
}
