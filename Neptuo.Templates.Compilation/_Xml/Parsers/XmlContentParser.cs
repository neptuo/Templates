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
                Helper helper = new Helper(content, context, builderRegistry);
                ICodeObject codeObject = ProcessNode(new XmlContentBuilderContext(this, helper, builderRegistry), helper.DocumentElement);
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

        public void AttachObservers(IContentBuilderContext context, IComponentCodeObject codeObject, IEnumerable<ParsedObserver> observers)
        {
            foreach (ParsedObserver observer in observers)
                observer.Observer.CreateBuilder().Parse(context, codeObject, observer.Attributes);
        }

        public ICodeObject ProcessNode(IContentBuilderContext context, IXmlNode node)
        {
            Helper helper = context.Helper;
            if (node.NodeType == XmlNodeType.Element)
            {
                IXmlElement element = (IXmlElement)node;
                IContentBuilderRegistry newBuilderRegistry = Utils.CreateChildRegistrator(builderRegistry, Utils.GetXmlNsNamespace(element));
                //if (Utils.NeedsServerProcessing(helper, newBuilderRegistry, element))
                //{
                //    FlushContent(helper);

                IContentBuilder builder = builderRegistry.GetComponentBuilder(element.Prefix, element.LocalName);
                if (builder == null)
                {
                    context.AddError(String.Format("Element {0} doesn't have registered builder.", element.Name));
                    return null;
                }

                ICodeObject codeObject = builder.Parse(new XmlContentBuilderContext(this, helper, newBuilderRegistry), element);
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
                return builderRegistry.GetLiteralBuilder().ParseText(new XmlContentBuilderContext(this, helper, builderRegistry), text.Text);
                //helper.Content.Append(text.Text);
            }
            else if (node.NodeType == XmlNodeType.Comment)
            {
                IXmlText text = (IXmlText)node;
                return builderRegistry.GetLiteralBuilder().ParseComment(new XmlContentBuilderContext(this, helper, builderRegistry), text.Text);
                //helper.Content.Append("<!--" + text.Text + "-->");
            }
            //FlushContent(helper);

            throw Guard.Exception.NotSupported("XmlContentParser supports only element, text or comment.");
        }

        private void FlushContent(Helper helper)
        {
            if (helper.Content.Length > 0)
                AppendPlainText(helper.Content.ToString(), helper);

            helper.Content.Clear();
        }

        private void AppendPlainText(string text, Helper helper)
        {
            //if (String.IsNullOrWhiteSpace(text))
            //    return;
            builderRegistry.GetLiteralBuilder().ParseText(new XmlContentBuilderContext(this, helper, builderRegistry), text);
        }
    }
}
