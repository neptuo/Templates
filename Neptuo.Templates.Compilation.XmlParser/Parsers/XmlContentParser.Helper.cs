using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Neptuo.Templates.Compilation.CodeObjects;

namespace Neptuo.Templates.Compilation.Parsers
{
    partial class XmlContentParser
    {
        /// <summary>
        /// Helper for xml content parser.
        /// </summary>
        public class Helper
        {
            public IPropertyDescriptor Parent { get; set; }
            public IContentParserContext Context { get; protected set; }
            public IXmlElement DocumentElement { get; protected set; }
            private IContentBuilderRegistry BuilderRegistry { get; set; }
            public StringBuilder Content { get; protected set; }

            public Helper(string xml, IContentParserContext context, IContentBuilderRegistry builderRegistry)
            {
                Context = context;
                BuilderRegistry = builderRegistry;
                Content = new StringBuilder();
                Parent = context.PropertyDescriptor;

                if (xml != null)
                    DocumentElement = XmlDocumentSupport.LoadXml(CreateRootElement(xml));
            }

            public void WithParent(IPropertyDescriptor parent, Action execute)
            {
                IPropertyDescriptor current = Parent;
                Parent = parent;
                execute();
                Parent = current;
            }

            public void FormatEmptyElement(IXmlElement element)
            {
                Content.AppendFormat("<{0}", element.Name);
                foreach (IXmlAttribute attribute in element.Attributes)
                    Content.AppendFormat(" {0}=\"{1}\"", attribute.Name, attribute.Value);

                Content.Append(" />");
            }

            public void FormatStartElement(IXmlElement element)
            {
                Content.AppendFormat("<{0}", element.Name);
                foreach (IXmlAttribute attribute in element.Attributes)
                    Content.AppendFormat(" {0}=\"{1}\"", attribute.Name, attribute.Value);

                Content.Append(">");
            }

            public void FormatEndElement(IXmlElement element)
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
