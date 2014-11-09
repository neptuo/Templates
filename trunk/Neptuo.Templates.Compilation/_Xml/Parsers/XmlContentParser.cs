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
        private IContentBuilderRegistry builderRegistry;

        public bool UseLinqApi { get; set; }

        public XmlContentParser(IContentBuilderRegistry builderRegistry)
        {
            this.builderRegistry = builderRegistry;
        }

        public ICodeObject Parse(ISourceContent content, IContentParserContext context)
        {
#if !DEBUG
            try
            {
#endif
            IXmlElement documentElement = CreateDocumentRoot(content);
            ICodeObject codeObject = ProcessNode(new XmlContentBuilderContext(context, this, builderRegistry), documentElement);
            //FlushContent(helper); - doesn't respect current parent

            return codeObject;
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

        public void AttachObservers(IContentBuilderContext context, IComponentCodeObject codeObject, IEnumerable<ObserverCollection.ItemValue> observers)
        {
            foreach (ObserverCollection.ItemValue observer in observers)
                observer.Observer.CreateBuilder().Parse(context, codeObject, observer.Attributes);
        }

        public ICodeObject ProcessNode(IContentBuilderContext context, IXmlNode node)
        {
            if (node.NodeType == XmlNodeType.Element)
            {
                IXmlElement element = (IXmlElement)node;
                IContentBuilderRegistry newBuilderRegistry = Utils.CreateChildRegistry(builderRegistry, Utils.GetXmlNsNamespace(element));
                //if (Utils.NeedsServerProcessing(helper, newBuilderRegistry, element))
                //{
                //    FlushContent(helper);

                IContentBuilder builder = builderRegistry.GetComponentBuilder(element.Prefix, element.LocalName);
                if (builder == null)
                {
                    context.AddError(String.Format("Element {0} doesn't have registered builder.", element.Name));
                    return null;
                }

                ICodeObject codeObject = builder.Parse(new XmlContentBuilderContext(context.ParserContext, this, newBuilderRegistry), element);
                return codeObject;

                //}
                //else if (element.IsEmpty)
                //{
                //    helper.FormatEmptyElement(element);
                //}
                //else
                //{
                //    helper.FormatStartElement(element);
                //    GenerateRecursive(helper, builderRegistry, element.ChildNodes);
                //    helper.FormatEndElement(element);
                //}
            }
            else if (node.NodeType == XmlNodeType.Text)
            {
                IXmlText text = (IXmlText)node;
                return builderRegistry.GetLiteralBuilder().ParseText(new XmlContentBuilderContext(context.ParserContext, this, builderRegistry), text.Text);
                //helper.Content.Append(text.Text);
            }
            else if (node.NodeType == XmlNodeType.Comment)
            {
                IXmlText text = (IXmlText)node;
                return builderRegistry.GetLiteralBuilder().ParseComment(new XmlContentBuilderContext(context.ParserContext, this, builderRegistry), text.Text);
                //helper.Content.Append("<!--" + text.Text + "-->");
            }
            //FlushContent(helper);

            throw Guard.Exception.NotSupported("XmlContentParser supports only element, text or comment.");
        }

        private void AppendPlainText(string text, IContentBuilderContext context)
        {
            //if (String.IsNullOrWhiteSpace(text))
            //    return;
            builderRegistry.GetLiteralBuilder().ParseText(new XmlContentBuilderContext(context.ParserContext, this, builderRegistry), text);
        }

        #region Creating document

        private IXmlElement CreateDocumentRoot(ISourceContent content)
        {
            string preparedXml = CreateRootElement(content.TextContent);
            IXmlElement documentElement;
            if (UseLinqApi)
                documentElement = XDocumentSupport.LoadXml(preparedXml);
            else
                documentElement = XDocumentSupport.LoadXml(preparedXml);

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
            result.Append("<NeptuoTemplatesRoot");
            foreach (NamespaceDeclaration entry in builderRegistry.GetRegisteredNamespaces())
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
            result.Append("</NeptuoTemplatesRoot>");

            return result.ToString();
        }

        #endregion
    }
}
