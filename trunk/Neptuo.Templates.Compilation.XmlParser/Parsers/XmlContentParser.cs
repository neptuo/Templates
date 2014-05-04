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
    public partial class XmlContentParser : IContentParser
    {
        private IContentBuilderRegistry builderRegistry;

        public XmlContentParser(IContentBuilderRegistry builderRegistry)
        {
            this.builderRegistry = builderRegistry;
        }

        public bool Parse(string content, IContentParserContext context)
        {
#if !DEBUG
            try
            {
#endif
                Helper helper = new Helper(content, context, builderRegistry);
                GenerateRecursive(helper, builderRegistry, helper.DocumentElement.ChildNodes);
                //FlushContent(helper); - doesn't respect current parent

                return true;
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
                    context.Errors.Add(new ErrorInfo(sourceException.LineNumber, sourceException.LinePosition, sourceException.Message));
                else
                    context.Errors.Add(new ExceptionErrorInfo(e));
            }
            return false;
#endif
        }

        public void ProcessContent(IContentBuilderContext context, IPropertyDescriptor propertyDescriptor, IEnumerable<IXmlNode> content)
        {
            context.Helper.WithParent(propertyDescriptor, () => GenerateRecursive(context.Helper, context.BuilderRegistry, content));
        }

        public void AttachObservers(IContentBuilderContext context, IComponentCodeObject codeObject, IEnumerable<ParsedObserver> observers)
        {
            foreach (ParsedObserver observer in observers)
                observer.Observer.CreateBuilder().Parse(context, codeObject, observer.Attributes);
        }

        private void GenerateRecursive(Helper helper, IContentBuilderRegistry buildeRegistry, IEnumerable<IXmlNode> childNodes)
        {
            foreach (IXmlNode node in childNodes)
            {
                if (node.NodeType == XmlNodeType.Element)
                {
                    IXmlElement element = (IXmlElement)node;
                    IContentBuilderRegistry newBuilderRegistry = Utils.CreateChildRegistrator(builderRegistry, Utils.GetXmlNsNamespace(element));
                    if (Utils.NeedsServerProcessing(helper, newBuilderRegistry, element))
                    {
                        FlushContent(helper);

                        IComponentBuilder builder = builderRegistry.GetComponentBuilder(element.Prefix, element.LocalName);

                        if (builder == null)
                            throw new ArgumentOutOfRangeException(element.Name, "This element doesn't have builder!"); //TODO: Add as error!
                            
                        builder.Parse(new XmlContentBuilderContext(this, helper, newBuilderRegistry), element);
                    }
                    else if (element.IsEmpty)
                    {
                        helper.FormatEmptyElement(element);
                    }
                    else
                    {
                        helper.FormatStartElement(element);
                        GenerateRecursive(helper, buildeRegistry, element.ChildNodes);
                        helper.FormatEndElement(element);
                    }
                }
                else if (node.NodeType == XmlNodeType.Text)
                {
                    IXmlText text = (IXmlText)node;
                    helper.Content.Append(text.Text);
                }
                else if (node.NodeType == XmlNodeType.Comment)
                {
                    IXmlText text = (IXmlText)node;
                    helper.Content.Append("<!--" + text.Text + "-->");
                }
            }
            FlushContent(helper);
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
            builderRegistry.GetLiteralBuilder().Parse(new XmlContentBuilderContext(this, helper, builderRegistry), text);
        }
    }
}
