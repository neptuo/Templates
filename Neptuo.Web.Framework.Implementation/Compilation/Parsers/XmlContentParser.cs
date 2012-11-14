using Neptuo.Web.Framework.Annotations;
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
                if (Utils.NeedsServerProcessing(helper.Registrator, node))
                {
                    AppendPlainText(helper.Content.ToString(), helper);
                    helper.Content.Clear();

                    if (node.GetType() == typeof(XmlElement))
                    {
                        XmlElement element = node as XmlElement;

                        Type controlType = helper.Registrator.GetControl(element.Prefix, element.LocalName);
                        if (controlType == null)
                            controlType = genericContentDescriptor.Type;

                        if (controlType != null)
                        {
                            BuilderAttribute attr = ReflectionHelper.GetAttribute<BuilderAttribute>(controlType);
                            if (attr != null && typeof(IXmlControlBuilder).IsAssignableFrom(attr.Type))
                            {
                                IXmlControlBuilder builder = (IXmlControlBuilder)Activator.CreateInstance(attr.Type);
                                builder.Parse(helper, controlType, element);
                            }
                            else 
                            {
                                GenerateControl(helper, controlType, element);
                            }
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
            if (String.IsNullOrWhiteSpace(text))
                return;

            text = text.Trim(); //TODO: Trim??

            //TODO: Test for LiteralControl (IsAssignableForm IControl/LiteralControl)
            helper.Parent.SetValue(new PlainValueCodeObject(text));
            //helper.Parent.AddProperty(new LiteralCodeObject(literalDescriptor, text));
        }

        public void GenerateControl(Helper helper, Type controlType, XmlElement element)
        {
            ControlCodeObject codeObject = new ControlCodeObject(controlType);
            helper.Parent.SetValue(codeObject);
            //helper.Parent.AddProperty(codeObject);

            if (controlType == genericContentDescriptor.Type)
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
            List<XmlAttribute> boundAttributes = new List<XmlAttribute>();

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
                            boundAttributes.Add(attribute);
                            bound = true;
                        }
                    }
                    //if (!bound && !String.IsNullOrWhiteSpace(attribute.Prefix) && !observerAttributes.Contains(attribute))
                    //    observerAttributes.Add(attribute);
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

            
            List<XmlAttribute> unboundAttributes = new List<XmlAttribute>();
            foreach (XmlAttribute attribute in element.Attributes)
            {
                if (!boundAttributes.Contains(attribute))
                    unboundAttributes.Add(attribute);
            }
            ProcessUnboundAttributes(helper, codeObject, unboundAttributes);
            
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

        private void ProcessUnboundAttributes(Helper helper, IComponentCodeObject codeObject, List<XmlAttribute> unboundAttributes)
        {
            ObserverList observers = new ObserverList();
            foreach (XmlAttribute attribute in unboundAttributes)
            {
                bool boundAttribute = false;

                if (attribute.Prefix.ToLowerInvariant() == "xmlns")
                {
                    if (attribute.Value.StartsWith("clr-namespace:"))
                    {
                        helper.Registrator.RegisterNamespace(attribute.LocalName, attribute.Value.Substring("clr-namespace:".Length));
                        //TODO: Register XMLNS, create temporal registrator or unregister XMLNS after this control
                        boundAttribute = true;
                    }
                }

                if (!boundAttribute)
                {
                    Type observerType = helper.Registrator.GetObserver(attribute.Prefix, attribute.LocalName);
                    if (observerType != null)
                    {
                        ObserverLivecycle livecycle = GetObserverLivecycle(observerType);
                        if (livecycle == ObserverLivecycle.PerControl && observers.ContainsKey(observerType))
                            observers[observerType].Attributes.Add(attribute);
                        else
                            observers.Add(new ParsedObserver(observerType, livecycle, attribute));

                        boundAttribute = true;
                    }
                }

                if (!boundAttribute && typeof(IAttributeCollection).IsAssignableFrom(codeObject.Type))
                    boundAttribute = BindAttributeCollection(helper, codeObject, codeObject, attribute.LocalName, attribute.Value);
            }

            foreach (ParsedObserver observer in observers)
                RegisterObserver(helper, codeObject, observer.Type, observer.Attributes, observer.Livecycle);
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
                            new PlainValueCodeObject(contentValue.ToString())
                        )
                    );
                }
            }
        }

        private void RegisterObserver(Helper helper, IComponentCodeObject codeObject, Type observerType, IEnumerable<XmlAttribute> attributes, ObserverLivecycle livecycle)
        {
            IObserverCodeObject observerObject = new ObserverCodeObject(observerType, livecycle);
            codeObject.Observers.Add(observerObject);

            List<XmlAttribute> boundAttributes = new List<XmlAttribute>();
            foreach (KeyValuePair<string, PropertyInfo> property in ControlHelper.GetProperties(observerType))
            {
                bool boundProperty = false;
                foreach (XmlAttribute attribute in attributes)
                {
                    if (property.Key.ToLowerInvariant() == attribute.LocalName.ToLowerInvariant())
                    {
                        IPropertyDescriptor propertyDescriptor = new SetPropertyDescriptor(property.Value);
                        bool result = helper.Context.ParserService.ProcessValue(attribute.Value, new DefaultParserServiceContext(helper.Context.ServiceProvider, propertyDescriptor));

                        if (!result)
                            result = BindPropertyDefaultValue(propertyDescriptor);

                        if (result)
                            observerObject.Properties.Add(propertyDescriptor);

                        boundAttributes.Add(attribute);
                        boundProperty = true;
                    }
                }

                if (!boundProperty)
                {
                    IPropertyDescriptor propertyDescriptor = new SetPropertyDescriptor(property.Value);
                    bool result = BindPropertyDefaultValue(propertyDescriptor);

                    if (result)
                        observerObject.Properties.Add(propertyDescriptor);
                }
            }

            List<XmlAttribute> unboundAttributes = new List<XmlAttribute>();
            foreach (XmlAttribute attribute in attributes)
            {
                if (!boundAttributes.Contains(attribute))
                    unboundAttributes.Add(attribute);
            }

            foreach (XmlAttribute attribute in unboundAttributes)
            {
                if (typeof(IAttributeCollection).IsAssignableFrom(observerObject.Type))
                    BindAttributeCollection(helper, observerObject, observerObject, attribute.LocalName, attribute.Value);
            }
        }

        private bool BindAttributeCollection(Helper helper, ITypeCodeObject typeCodeObject, IPropertiesCodeObject propertiesCodeObject, string name, string value)
        {
            MethodInfo method = typeCodeObject.Type.GetMethod(TypeHelper.MethodName<IAttributeCollection, string, string>(a => a.SetAttribute));
            MethodInvokePropertyDescriptor propertyDescriptor = new MethodInvokePropertyDescriptor(method);
            propertyDescriptor.SetValue(new PlainValueCodeObject(name));

            bool result = helper.Context.ParserService.ProcessValue(
                value,
                new DefaultParserServiceContext(helper.Context.ServiceProvider, propertyDescriptor)
            );
            if (result)
                propertiesCodeObject.Properties.Add(propertyDescriptor);

            //TODO: Else NOT result?
            return result;
        }

        private ObserverLivecycle GetObserverLivecycle(Type observerType)
        {
            ObserverLivecycle livecycle = ObserverLivecycle.PerControl;

            ObserverAttribute observerAttribute = ReflectionHelper.GetAttribute<ObserverAttribute>(observerType);
            if (observerAttribute != null)
                livecycle = observerAttribute.Livecycle;

            return livecycle;
        }
    }
}
