using Neptuo.ComponentModel;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Data;
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
    public partial class XmlContentParser : IContentParser
    {
        public static string FakeRootElementName = "NeptuoTemplatesRoot";

        private readonly IContentBuilder componentFactory;
        private readonly ILiteralBuilder literalFactory;

        public bool UseLinqApi { get; set; }

        public XmlContentParser(IContentBuilder componentFactory, ILiteralBuilder literalFactory, bool useLinqApi = false)
        {
            Guard.NotNull(componentFactory, "componentFactory");
            Guard.NotNull(literalFactory, "literalFactory");
            this.componentFactory = componentFactory;
            this.literalFactory = literalFactory;
            UseLinqApi = useLinqApi;
        }

        public virtual ICodeObject Parse(ISourceContent content, IContentParserContext context)
        {
#if !DEBUG
            try
            {
#endif
            IXmlElement documentElement = CreateDocumentRoot(content);
            IEnumerable<ICodeObject> codeObject = TryProcessNode(new XmlContentBuilderContext(context, this), documentElement);
            //FlushContent(helper); - doesn't respect current parent

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
            catch (Exception e)
            {
                ISourceCodeException sourceException = e as ISourceCodeException;
                if (sourceException != null)
                    context.Errors.Add(new ErrorInfo(sourceException.LineNumber, sourceException.LineIndex, sourceException.Message));
                else
                    context.Errors.Add(new ExceptionErrorInfo(e));
            }
            return null;
#endif
        }

        public IEnumerable<ICodeObject> TryProcessNode(IContentBuilderContext context, IXmlNode node)
        {
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

            throw Guard.Exception.NotSupported("XmlContentParser supports only element, text or comment.");
        }

        protected virtual IEnumerable<ICodeObject> TryProcessElement(IContentBuilderContext context, IXmlElement element)
        {
            return componentFactory.TryParse(new XmlContentBuilderContext(context), element);
        }

        protected virtual IEnumerable<ICodeObject> TryProcessText(IContentBuilderContext context, IXmlText text)
        {
            return literalFactory.TryParseText(new XmlContentBuilderContext(context), text.Text);
        }

        protected virtual IEnumerable<ICodeObject> TryProcessComment(IContentBuilderContext context, IXmlText text)
        {
            return literalFactory.TryParseComment(new XmlContentBuilderContext(context), text.Text);
        }

        #region Creating document

        private IXmlElement CreateDocumentRoot(ISourceContent content)
        {
            string preparedXml = CreateRootElement(content.TextContent);
            IXmlElement documentElement;
            if (UseLinqApi)
                documentElement = XDocumentSupport.LoadXml(preparedXml);
            else
                documentElement = XmlDocumentSupport.LoadXml(preparedXml);

            return documentElement;
        }

        private string CreateRootElement(string content)
        {
            // If we start with property xml definition name, we are assuming that content is prefectly valid.
            if (content.StartsWith("<?xml"))
                return content;

            // Otherwise we create root element with necessary namespace mappings.
            HashSet<string> usedPrefixes = new HashSet<string>();
            StringBuilder result = new StringBuilder();

            result.Append("<?xml version=\"1.0\" ?>");
            result.Append("<" + FakeRootElementName);
            //foreach (NamespaceDeclaration entry in Enumerable.Empty<NamespaceDeclaration>()) //TODO: Move this to "View Normalizer".
            //{
            //    if (usedPrefixes.Add(entry.Prefix))
            //    {
            //        result.AppendFormat(
            //            " xmlns{0}=\"clr-namespace:{1}\"",
            //            String.IsNullOrEmpty(entry.Prefix) ? String.Empty : String.Format(":{0}", entry.Prefix),
            //            entry.Namespace
            //        );
            //    }
            //}
            result.Append(">");
            result.Append(content);
            result.Append("</" + FakeRootElementName + ">");

            return result.ToString();
        }

        #endregion
    }
}
