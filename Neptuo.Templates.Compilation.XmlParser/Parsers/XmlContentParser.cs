﻿using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Data;
using Neptuo.Xml;
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
    public partial class XmlContentParser : BaseParser, IContentParser
    {
        private ILiteralBuilder literalBuilder;
        private IComponentBuilder genericContentBuilder;
        private IBuilderRegistry builderRegistry;

        public XmlContentParser(ILiteralBuilder literalBuilder, IComponentBuilder genericContentBuilder, IBuilderRegistry builderRegistry)
        {
            this.literalBuilder = literalBuilder;
            this.genericContentBuilder = genericContentBuilder;
            this.builderRegistry = builderRegistry;
        }

        public bool Parse(string content, IContentParserContext context)
        {
            try
            {
                Helper helper = new Helper(content, context, builderRegistry);
                GenerateRecursive(helper, helper.Document.DocumentElement.ChildNodes.ToEnumerable());

                return true;
            }
            catch (XmlException e)
            {
                context.Errors.Add(new ErrorInfo(e.LineNumber, e.LinePosition, e.Message));
#if DEBUG
                throw e;
#endif
            }
            catch (Exception e)
            {
                context.Errors.Add(new ErrorInfo(e.Message));
#if DEBUG
                throw e;
#endif
            }
#if !DEBUG
            return false;
#endif
        }

        public void ProcessContent(IBuilderContext context, IPropertyDescriptor propertyDescriptor, IEnumerable<XmlNode> content)
        {
            context.Helper.WithParent(propertyDescriptor, () => GenerateRecursive(context.Helper, content));
        }

        public void AttachObservers(IBuilderContext context, IComponentCodeObject codeObject, IEnumerable<ParsedObserver> observers)
        {
            foreach (ParsedObserver observer in observers)
                observer.Observer.CreateBuilder().Parse(context, codeObject, observer.Attributes);
        }

        private void GenerateRecursive(Helper helper, IEnumerable<XmlNode> childNodes)
        {
            foreach (XmlNode node in childNodes)
            {
                XmlElement element = node as XmlElement;
                if (element != null)
                {
                    IBuilderRegistry newBuilderRegistry = Utils.CreateChildRegistrator(helper.BuilderRegistry, Utils.GetXmlNsNamespace(element));
                    if (Utils.NeedsServerProcessing(newBuilderRegistry, element))
                    {
                        AppendPlainText(helper.Content.ToString(), helper);
                        helper.Content.Clear();

                        IBuilderRegistry currentBuilderRegistry = helper.BuilderRegistry;
                        helper.BuilderRegistry = newBuilderRegistry;

                        IComponentBuilder builder = helper.BuilderRegistry.GetComponentBuilder(element.Prefix, element.LocalName);
                        if (builder == null)
                            throw new ArgumentOutOfRangeException(element.Name, "This element doesn't has builder!"); //TODO: Add as error!
                            
                        builder.Parse(CreateBuilderContext(helper), element);
                        helper.BuilderRegistry = currentBuilderRegistry;
                    }
                    else if (element.IsEmpty)
                    {
                        helper.FormatEmptyElement(element);
                    }
                    else
                    {
                        helper.FormatStartElement(element);
                        GenerateRecursive(helper, element.ChildNodes.ToEnumerable());
                        helper.FormatEndElement(element);
                    }
                }
                else if (node.GetType() == typeof(XmlText))
                {
                    XmlText text = node as XmlText;
                    helper.Content.Append(text.InnerText);
                }
            }
            AppendPlainText(helper.Content.ToString(), helper);
            helper.Content.Clear();
        }

        private IBuilderContext CreateBuilderContext(Helper helper)
        {
            return XmlBuilderContext.Create()
                .SetParser(this)
                .SetHelper(helper)
                .SetGenericContent(genericContentBuilder)
                .SetLiteralBuilder(literalBuilder);
        }

        private void AppendPlainText(string text, Helper helper)
        {
            if (String.IsNullOrWhiteSpace(text))
                return;

            literalBuilder.Parse(CreateBuilderContext(helper), text);

            //text = text.Trim(); //TODO: Trim??

            //Type propertyType = helper.Parent.Property.PropertyType;
            //if (propertyType.IsGenericType)
            //    propertyType = propertyType.GetGenericArguments()[0];

            //ICodeObject result = null;
            //if (propertyType.IsAssignableFrom(typeof(string)))
            //{
            //    result = new PlainValueCodeObject(text);
            //}
            //else if (propertyType.IsAssignableFrom(literalDescriptor.Type) || propertyType.IsAssignableFrom(typeof(IControl)))
            //{
            //    ControlCodeObject codeObject = new ControlCodeObject(literalDescriptor.Type);
            //    codeObject.Properties.Add(new SetPropertyDescriptor(literalDescriptor.Type.GetProperty(literalDescriptor.TextProperty), new PlainValueCodeObject(text)));
            //    result = codeObject;
            //}

            //helper.Parent.SetValue(result);
        }
    }
}
