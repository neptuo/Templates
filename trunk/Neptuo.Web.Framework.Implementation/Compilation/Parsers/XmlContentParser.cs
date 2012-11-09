﻿using Neptuo.Web.Framework.Annotations;
using Neptuo.Web.Framework.Compilation.CodeObjects;
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
using TypeConverter = Neptuo.Web.Framework.Utils.StringConverter;

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
            Helper helper = new Helper(content, context);

            GenerateRecursive(helper, helper.Document.DocumentElement.ChildNodes.ToEnumerable());

            return true;
        }

        private void GenerateRecursive(Helper helper, IEnumerable<XmlNode> childNodes)
        {
            foreach (XmlNode node in childNodes)
            {
                if (Utils.NeedsServerProcessing(node))
                {
                    AppendPlainText(helper.Content.ToString(), helper);
                    helper.Content.Clear();

                    if (node.GetType() == typeof(XmlElement))
                    {
                        XmlElement element = node as XmlElement;

                        Type controlType;
                        if (String.IsNullOrWhiteSpace(element.Prefix))
                            controlType = genericContentDescriptor.Type;
                        else
                            controlType = helper.Registrator.GetControl(element.Prefix, element.LocalName);

                        if (controlType != null)
                        {
                            //BuilderAttribute attr = BuilderAttribute.GetAttribute(controlType);
                            //if (attr != null)
                            //{
                            //    IXmlControlBuilder builder = Activator.CreateInstance(attr.BuilderType) as IXmlControlBuilder;
                            //    if (builder != null)
                            //    {
                            //        builder.GenerateControl(controlType, element, new XmlBuilderContext(helper.Context, this));
                            //        return;
                            //    }
                            //}

                            GenerateControl(helper, controlType, element);
                        }
                    }
                }
                else
                {
                    if (node.GetType() == typeof(XmlElement))
                    {
                        XmlElement element = node as XmlElement;
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
                    else if (node.GetType() == typeof(XmlText))
                    {
                        XmlText text = node as XmlText;
                        helper.Content.Append(text.InnerText);
                    }
                }
            }
            AppendPlainText(helper.Content.ToString(), helper);
            helper.Content.Clear();
        }

        private void AppendPlainText(string text, Helper helper)
        {
            //bool canUse = helper.Parent.PropertyInfo.RequiredType.IsAssignableFrom(literalDescriptor.Type);
            //if (!canUse)
            //    return;

            if (String.IsNullOrWhiteSpace(text))
                return;

            text = text.Trim();//TODO: Trim??

            helper.Parent.SetValue(new PlainValueCodeObject(text));
            //helper.Parent.AddProperty(new LiteralCodeObject(literalDescriptor, text));
        }

        public void GenerateControl(Helper helper, Type controlType, XmlElement element)
        {
            ControlCodeObject codeObject = new ControlCodeObject(controlType);
            helper.Parent.SetValue(codeObject);
            //helper.Parent.AddProperty(codeObject);

            if (String.IsNullOrWhiteSpace(element.Prefix))
            {
                codeObject.Properties.Add(
                    new SetPropertyDescriptor(
                        controlType.GetProperty(genericContentDescriptor.TagNameProperty), 
                        new PlainValueCodeObject(element.Name)
                    )
                );
            }

            BindProperties(helper, codeObject, element);
        }

        private void BindProperties(Helper helper, IComponentCodeObject codeObject, XmlElement element)
        {
            HashSet<string> boundProperies = new HashSet<string>();
            PropertyInfo defaultProperty = ControlHelper.GetDefaultProperty(codeObject.Type);
            List<XmlAttribute> observerAttributes = new List<XmlAttribute>();

            foreach (KeyValuePair<string, PropertyInfo> item in ControlHelper.GetProperties(codeObject.Type))
            {
                bool bound = false;
                string propertyName = item.Key.ToLowerInvariant();
                foreach (XmlAttribute attribute in element.Attributes)
                {
                    if (propertyName == attribute.Name.ToLowerInvariant())
                    {
                        IPropertyDescriptor propertyDescriptor = new SetPropertyDescriptor(item.Value);
                        bool result = helper.Context.ParserService.ProcessValue(
                            attribute.Value, 
                            new DefaultParserServiceContext(helper.Context.ServiceProvider, propertyDescriptor)
                        );

                        if (!result)
                            result = BindPropertyDefaultValue(propertyDescriptor);

                        if (result)
                        {
                            codeObject.Properties.Add(propertyDescriptor);
                            boundProperies.Add(propertyName);
                            bound = true;
                        }
                    }

                    if (!bound && !String.IsNullOrWhiteSpace(attribute.Prefix) && !observerAttributes.Contains(attribute))
                        observerAttributes.Add(attribute);
                }

                XmlNode child;
                if (!bound && Utils.FindChildNode(element, propertyName, out child))
                {
                    ResolvePropertyValue(helper, codeObject, item.Value, child.ChildNodes.ToEnumerable());
                    //ResolvePropertyValue(control, controlType, context, item.Value, args.ParsedItem.Content);
                    boundProperies.Add(propertyName);
                    bound = true;
                }

                if (!bound && item.Value != defaultProperty)
                {
                    IPropertyDescriptor propertyDescriptor = new SetPropertyDescriptor(item.Value);
                    if (BindPropertyDefaultValue(propertyDescriptor))
                    {
                        codeObject.Properties.Add(propertyDescriptor);
                        boundProperies.Add(propertyName);
                        bound = true;
                    }
                }
            }

            foreach (XmlAttribute attribute in observerAttributes)
            {
                Type observerType = helper.Registrator.GetObserver(attribute.Prefix, attribute.LocalName);
                if (observerType != null)
                {
                    //TODO: Register observer
                    RegisterObserver(helper, codeObject, observerType, attribute);
                }
            }

            if (defaultProperty != null && !boundProperies.Contains(defaultProperty.Name.ToLowerInvariant()))
            {
                IEnumerable<XmlNode> defaultChildNodes = Utils.FindNotUsedChildNodes(element, boundProperies);
                if (defaultChildNodes.Any())
                {
                    ResolvePropertyValue(helper, codeObject, defaultProperty, defaultChildNodes);
                }
                else
                {
                    IPropertyDescriptor propertyDescriptor = new SetPropertyDescriptor(defaultProperty);
                    if (BindPropertyDefaultValue(propertyDescriptor))
                        codeObject.Properties.Add(propertyDescriptor);
                }
            }
        }

        private void ResolvePropertyValue(Helper helper, IPropertiesCodeObject codeObject, PropertyInfo prop, IEnumerable<XmlNode> content)
        {
            if (typeof(ICollection).IsAssignableFrom(prop.PropertyType)
                || typeof(IEnumerable).IsAssignableFrom(prop.PropertyType)
                || typeof(ICollection<>).IsAssignableFrom(prop.PropertyType.GetGenericTypeDefinition())
            )
            {
                //Collection item
                IPropertyDescriptor propertyDescriptor = new ListAddPropertyDescriptor(prop);
                codeObject.Properties.Add(propertyDescriptor);
                helper.WithParent(propertyDescriptor, () => GenerateRecursive(helper, content));
            }
            else
            {
                // Count elements
                IEnumerable<XmlElement> elements = content.OfType<XmlElement>();
                if (elements.Any())
                {
                    // One XmlElement is ok
                    if (elements.Count() == 1)
                    {
                        IPropertyDescriptor propertyDescriptor = new SetPropertyDescriptor(prop);
                        codeObject.Properties.Add(propertyDescriptor);
                        helper.WithParent(propertyDescriptor, () => GenerateRecursive(helper, content));
                    }
                    else
                    {
                        //More elements can't be bound!
                        throw new ArgumentException("Unbindable property!");
                    }
                }
                else
                {
                    //Get string and add as plain value
                    StringBuilder contentValue = new StringBuilder();
                    foreach (XmlNode node in content)
                        contentValue.Append(node.OuterXml);

                    codeObject.Properties.Add(
                        new SetPropertyDescriptor(
                            prop,
                            new PlainValueCodeObject(TypeConverter.Convert(contentValue.ToString(), prop.PropertyType))
                        )
                    );
                }
            }
        }

        private void RegisterObserver(Helper helper, IComponentCodeObject codeObject, Type observerType, XmlAttribute attribute)
        {
            ObserverLivecycle livecycle = ObserverLivecycle.PerControl;

            ObserverAttribute observerAttribute = ObserverAttribute.GetAttribute(observerType);
            if (observerAttribute != null)
                livecycle = observerAttribute.Livecycle;

            //TODO: Nevytvářet vždy nového, ale hledat již existující podle životního cyklu!
            //TODO: Nebindovat defaultproperty, kvůli sdílení instance
            IObserverCodeObject observerObject = new ObserverCodeObject(observerType, livecycle);
            codeObject.Observers.Add(observerObject);

            foreach (KeyValuePair<string, PropertyInfo> property in ControlHelper.GetProperties(observerType))
            {
                if (property.Key.ToLowerInvariant() == attribute.LocalName.ToLowerInvariant())
                {
                    IPropertyDescriptor propertyDescriptor = new SetPropertyDescriptor(property.Value);
                    bool result = helper.Context.ParserService.ProcessValue(attribute.Value, new DefaultParserServiceContext(helper.Context.ServiceProvider, propertyDescriptor));

                    if (!result)
                        result = BindPropertyDefaultValue(propertyDescriptor);

                    if (result)
                        observerObject.Properties.Add(propertyDescriptor);
                }
                else
                {
                    IPropertyDescriptor propertyDescriptor = new SetPropertyDescriptor(property.Value);
                    bool result = BindPropertyDefaultValue(propertyDescriptor);

                    if (result)
                        observerObject.Properties.Add(propertyDescriptor);
                }
            }
        }
    }
}