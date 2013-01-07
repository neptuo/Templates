using Neptuo.Web.Framework.Compilation.CodeObjects;
using Neptuo.Web.Framework.Compilation.Data;
using Neptuo.Web.Framework.Controls;
using Neptuo.Web.Framework.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Neptuo.Web.Framework.Compilation.Parsers
{
    public partial class XmlContentParser : BaseParser, IContentParser
    {
        private LiteralTypeDescriptor literalDescriptor;
        private GenericContentTypeDescriptor genericContentDescriptor;

        public XmlContentParser(LiteralTypeDescriptor literalDescriptor, GenericContentTypeDescriptor genericContentDescriptor)
        {
            this.literalDescriptor = literalDescriptor;
            this.genericContentDescriptor = genericContentDescriptor;
        }

        public bool Parse(string content, IContentParserContext context)
        {
            try
            {
                Helper helper = new Helper(content, context);

                GenerateRecursive(helper, helper.Document.DocumentElement.ChildNodes.ToEnumerable());

                return true;
            }
            catch (XmlException e)
            {
                context.Errors.Add(new ErrorInfo(e.LineNumber, e.LinePosition, e.Message));
            }
            catch (Exception e)
            {
                context.Errors.Add(new ErrorInfo(e.Message));
            }
            return false;
        }

        public void ProcessContent(IXmlBuilderContext context, IPropertyDescriptor propertyDescriptor, IEnumerable<XmlNode> content)
        {
            context.Helper.WithParent(propertyDescriptor, () => GenerateRecursive(context.Helper, content));
        }

        public void AttachObservers(IXmlBuilderContext context, IComponentCodeObject codeObject, IEnumerable<ParsedObserver> observers)
        {
            foreach (ParsedObserver observer in observers)
            {
                IXmlObserverBuilder builder = ControlHelper.ResolveBuilder<IXmlObserverBuilder>(observer.Type, context.ParserContext.DependencyProvider, () => new DefaultXmlObserverBuilder());
                if (builder != null)
                    builder.Parse(context, codeObject, observer.Type, observer.Attributes, observer.Livecycle);
                else
                    throw new NotImplementedException("No builder for observer!");
            }
        }

        private void GenerateRecursive(Helper helper, IEnumerable<XmlNode> childNodes)
        {
            foreach (XmlNode node in childNodes)
            {
                XmlElement element = node as XmlElement;
                if (element != null)
                {
                    IRegistrator newRegistrator = Utils.CreateChildRegistrator(helper.Registrator, Utils.GetXmlNsClrNamespace(element));
                    if (Utils.NeedsServerProcessing(newRegistrator, element))
                    {
                        AppendPlainText(helper.Content.ToString(), helper);
                        helper.Content.Clear();

                        IRegistrator currentRegistrator = helper.Registrator;
                        helper.Registrator = newRegistrator;

                        Type controlType = helper.Registrator.GetControl(element.Prefix, element.LocalName);
                        if (controlType == null)
                            controlType = genericContentDescriptor.Type;

                        if (controlType != null)
                        {
                            IXmlControlBuilder builder = ControlHelper.ResolveBuilder<IXmlControlBuilder>(controlType, helper.Context.DependencyProvider, () => new DefaultXmlControlBuilder());
                            if (builder != null)
                                builder.Parse(CreateBuilderContext(helper), controlType, element);
                            else
                                //GenerateControl(helper, controlType, element);
                                throw new NotImplementedException("No builder for control!");
                        }

                        helper.Registrator = currentRegistrator;
                    }
                    else
                    {
                        if (element.IsEmpty)
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

        private IXmlBuilderContext CreateBuilderContext(Helper helper)
        {
            return DefaultXmlBuilderContext.Create()
                .SetParserContext(helper.Context)
                .SetParser(this)
                .SetHelper(helper)
                .SetParent(helper.Parent)
                .SetGenericContentTypeDescriptor(genericContentDescriptor)
                .SetLiteralTypeDescriptor(literalDescriptor)
                .SetRegistrator(helper.Registrator);
        }

        private void AppendPlainText(string text, Helper helper)
        {
            if (String.IsNullOrWhiteSpace(text))
                return;

            text = text.Trim(); //TODO: Trim??

            Type propertyType = helper.Parent.Property.PropertyType;
            if (propertyType.IsGenericType)
                propertyType = propertyType.GetGenericArguments()[0];

            ICodeObject result = null;
            if (propertyType.IsAssignableFrom(typeof(string)))
            {
                result = new PlainValueCodeObject(text);
            }
            else if (propertyType.IsAssignableFrom(literalDescriptor.Type) || propertyType.IsAssignableFrom(typeof(IControl)))
            {
                ControlCodeObject codeObject = new ControlCodeObject(literalDescriptor.Type);
                codeObject.Properties.Add(new SetPropertyDescriptor(literalDescriptor.Type.GetProperty(literalDescriptor.TextProperty), new PlainValueCodeObject(text)));
                result = codeObject;
            }

            helper.Parent.SetValue(result);
        }

        public bool BindAttributeCollection(IXmlBuilderContext context, ITypeCodeObject typeCodeObject, IPropertiesCodeObject propertiesCodeObject, string name, string value)
        {
            MethodInfo method = typeCodeObject.Type.GetMethod(TypeHelper.MethodName<IAttributeCollection, string, string>(a => a.SetAttribute));
            MethodInvokePropertyDescriptor propertyDescriptor = new MethodInvokePropertyDescriptor(method);
            propertyDescriptor.SetValue(new PlainValueCodeObject(name));

            bool result = context.ParserContext.ParserService.ProcessValue(
                value,
                new DefaultParserServiceContext(context.ParserContext.DependencyProvider, propertyDescriptor, context.ParserContext.Errors)
            );
            if (result)
                propertiesCodeObject.Properties.Add(propertyDescriptor);

            //TODO: Else NOT result?
            return result;
        }

        public ObserverLivecycle GetObserverLivecycle(Type observerType)
        {
            ObserverLivecycle livecycle = ObserverLivecycle.PerControl;

            ObserverAttribute observerAttribute = ReflectionHelper.GetAttribute<ObserverAttribute>(observerType);
            if (observerAttribute != null)
                livecycle = observerAttribute.Livecycle;

            return livecycle;
        }
    }
}
