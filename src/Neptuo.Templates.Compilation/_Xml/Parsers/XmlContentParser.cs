﻿using Neptuo.Compilers.Errors;
using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// XML content parser implementation.
    /// </summary>
    public class XmlContentParser : IContentParser
    {
        public static string FakeRootElementName = "NeptuoTemplatesRoot";

        private readonly IParserRegistry registry;

        public bool UseLinqApi { get; set; }

        public XmlContentParser(IParserRegistry registry, bool useLinqApi = false)
        {
            Ensure.NotNull(registry, "registry");
            this.registry = registry;
            UseLinqApi = useLinqApi;
        }

        public virtual ICodeObject Parse(ISourceContent content, IContentParserContext context)
        {
#if !DEBUG
            try
            {
#endif
            IXmlElement documentElement = CreateDocumentRoot(content);
            IEnumerable<ICodeObject> codeObject = TryProcessNode(new XmlContentBuilderContext(context, this, registry), documentElement);
            //FlushContent(helper); - doesn't respect current parent

            if (codeObject == null)
                return null;

            if (codeObject.Count() > 1)
            {
                context.Errors.Add(new ErrorInfo(0, 0, "Root of AST can contain only single item."));
                return null;
            }

            return codeObject.FirstOrDefault();
#if !DEBUG
            }
            catch (XmlException e)
            {
                context.Errors.Add(new ExceptionErrorInfo(e.LineNumber - 1, e.LinePosition, e));
            }
            return null;
#endif
        }

        public IEnumerable<ICodeObject> TryProcessNode(IContentBuilderContext context, IXmlNode node)
        {
            context = new XmlContentBuilderContext(context);

            if (node.NodeType == XmlNodeType.Element)
            {
                IXmlElement element = (IXmlElement)node;
                return TryProcessElement(context, element);
            }
            else if (node.NodeType == XmlNodeType.Text)
            {
                IXmlText text = (IXmlText)node;
                return TryProcessText(context, text);
            }
            else if (node.NodeType == XmlNodeType.Comment)
            {
                IXmlText text = (IXmlText)node;
                return TryProcessComment(context, text);
            }

            throw Ensure.Exception.NotSupported("XmlContentParser supports only element, text or comment.");
        }

        protected virtual IEnumerable<ICodeObject> TryProcessElement(IContentBuilderContext context, IXmlElement element)
        {
            return context.Registry.WithContentBuilder().TryParse(new XmlContentBuilderContext(context), element);
        }

        protected virtual IEnumerable<ICodeObject> TryProcessText(IContentBuilderContext context, IXmlText text)
        {
            return context.Registry.WithLiteralBuilder().TryParseText(new XmlContentBuilderContext(context), text.Text);
        }

        protected virtual IEnumerable<ICodeObject> TryProcessComment(IContentBuilderContext context, IXmlText text)
        {
            return context.Registry.WithLiteralBuilder().TryParseComment(new XmlContentBuilderContext(context), text.Text);
        }

        #region Creating document

        private IXmlElement CreateDocumentRoot(ISourceContent content)
        {
            string preparedXml = PrepareXmlContent(content.TextContent, registry);
            IXmlElement documentElement;
            if (UseLinqApi)
                documentElement = XDocumentSupport.LoadXml(preparedXml);
            else
                documentElement = XmlDocumentSupport.LoadXml(preparedXml);

            return documentElement;
        }

        private string PrepareXmlContent(string content, IParserRegistry registry)
        {
            // If we start with property xml definition name, we are assuming that content is prefectly valid.
            if (content.StartsWith("<?xml"))
                return content;

            // Otherwise we create root element with necessary namespace mappings.
            HashSet<string> usedPrefixes = new HashSet<string>();
            StringBuilder result = new StringBuilder();

            result.Append("<?xml version=\"1.0\" ?>");
            result.Append("<" + FakeRootElementName);

            if (registry.HasUsedNamespaces())
            {
                foreach (NamespaceDeclaration entry in registry.WithUsedNamespaces())
                {
                    if (usedPrefixes.Add(entry.Prefix))
                    {
                        result.AppendFormat(
                            " xmlns{0}=\"clr-namespace:{1}\"",
                            String.IsNullOrEmpty(entry.Prefix) ? String.Empty : String.Format(":{0}", entry.Prefix),
                            entry.NamespaceName
                        );
                    }
                }
            }

            result.Append(">");
            result.Append(content);
            result.Append("</" + FakeRootElementName + ">");

            return result.ToString();
        }

        #endregion
    }
}
