using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Neptuo.Web.Framework.Utils;

namespace Neptuo.Web.Framework.Compilation
{
    partial class XmlContentParser
    {
        public class Helper
        {
            public ICodeObject Parent { get; set; }
            public IParserContext Context { get; protected set; }
            public XmlDocument Document { get; protected set; }
            public IRegistrator Registrator { get; protected set; }
            public StringBuilder Content { get; protected set; }

            public Helper(string xml, IParserContext context)
            {
                Context = context;
                Registrator = Context.ServiceProvider.GetService<IRegistrator>();
                Content = new StringBuilder();
                if (!(context.RootObject is IRootCodeObject))
                {
                    Parent = new RootCodeObject();
                    context.RootObject.AddProperty(Parent);
                }
                else
                {
                    Parent = (IRootCodeObject)context.RootObject;
                }

                if (xml != null)
                {
                    Document = new XmlDocument();
                    Document.LoadXml(CreateRootElement(xml));
                }
            }

            public void WithParent(ICodeObject parent, Action execute)
            {
                ICodeObject current = Parent;
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
                result.Append("<Neptuo-Web-Framework-Root");
                foreach (RegisteredNamespace entry in Registrator.GetRegisteredNamespaces())
                {
                    if (usedPrefixes.Add(entry.Prefix))
                        result.AppendFormat(" xmlns:{0}=\"clr-namespace:{1}\"", entry.Prefix, entry.Namespace);
                }
                result.Append(">");
                result.Append(content);
                result.Append("</Neptuo-Web-Framework-Root>");

                return result.ToString();
            }
        }
    }
}
