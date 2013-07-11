using Neptuo.Web.Framework.Compilation.CodeObjects;
using Neptuo.Web.Framework.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Neptuo.Web.Framework.Compilation.Parsers
{
    public class DefaultXmlControlBuilder : IXmlCodeObjectBuilder
    {
        public void Parse(IXmlBuilderContext context, Type controlType, XmlElement element)
        {
            ControlCodeObject codeObject = new ControlCodeObject(controlType);
            context.Parent.SetValue(codeObject);

            if (controlType == context.GenericContentTypeDescriptor.Type)
            {
                codeObject.Properties.Add(
                    new SetPropertyDescriptor(
                        controlType.GetProperty(context.GenericContentTypeDescriptor.TagNameProperty),
                        new PlainValueCodeObject(element.Name)
                    )
                );
            }

            BindProperties(context, codeObject, element);
        }

        protected void BindProperties(IXmlBuilderContext context, IComponentCodeObject codeObject, XmlElement element)
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
                        bool result = context.ParserContext.ParserService.ProcessValue(
                            attribute.Value,
                            new DefaultParserServiceContext(context.ParserContext.DependencyProvider, propertyDescriptor, context.ParserContext.Errors)
                        );

                        if (!result)
                            result = context.Parser.BindPropertyDefaultValue(propertyDescriptor);

                        if (result)
                        {
                            codeObject.Properties.Add(propertyDescriptor);
                            boundProperies.Add(propertyName);
                            boundAttributes.Add(attribute);
                            bound = true;
                        }
                    }
                }

                XmlNode child;
                if (!bound && XmlContentParser.Utils.FindChildNode(element, propertyName, out child))
                {
                    ResolvePropertyValue(context, codeObject, item.Value, child.ChildNodes.ToEnumerable());
                    boundProperies.Add(propertyName);
                    bound = true;
                }

                if (!bound && item.Value != defaultProperty)
                {
                    IPropertyDescriptor propertyDescriptor = new SetPropertyDescriptor(item.Value);
                    if (context.Parser.BindPropertyDefaultValue(propertyDescriptor))
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
            ProcessUnboundAttributes(context, codeObject, unboundAttributes);

            if (defaultProperty != null && !boundProperies.Contains(defaultProperty.Name.ToLowerInvariant()))
            {
                IEnumerable<XmlNode> defaultChildNodes = XmlContentParser.Utils.FindNotUsedChildNodes(element, boundProperies);
                if (defaultChildNodes.Any())
                {
                    ResolvePropertyValue(context, codeObject, defaultProperty, defaultChildNodes);
                }
                else
                {
                    IPropertyDescriptor propertyDescriptor = new SetPropertyDescriptor(defaultProperty);
                    if (context.Parser.BindPropertyDefaultValue(propertyDescriptor))
                        codeObject.Properties.Add(propertyDescriptor);
                }
            }
        }

        protected void ProcessUnboundAttributes(IXmlBuilderContext context, IComponentCodeObject codeObject, List<XmlAttribute> unboundAttributes)
        {
            XmlContentParser.ObserverList observers = new XmlContentParser.ObserverList();
            foreach (XmlAttribute attribute in unboundAttributes)
            {
                bool boundAttribute = false;

                if (attribute.Prefix.ToLowerInvariant() == "xmlns")
                    boundAttribute = true;

                if (!boundAttribute)
                {
                    Type observerType = context.Registrator.GetObserver(attribute.Prefix, attribute.LocalName);
                    if (observerType != null)
                    {
                        ObserverLivecycle livecycle = context.Parser.GetObserverLivecycle(observerType);
                        if (livecycle == ObserverLivecycle.PerControl && observers.ContainsKey(observerType))
                            observers[observerType].Attributes.Add(attribute);
                        else
                            observers.Add(new XmlContentParser.ParsedObserver(observerType, livecycle, attribute));

                        boundAttribute = true;
                    }
                }

                if (!boundAttribute && typeof(IAttributeCollection).IsAssignableFrom(codeObject.Type))
                    boundAttribute = context.Parser.BindAttributeCollection(context, codeObject, codeObject, attribute.LocalName, attribute.Value);
            }
            
            context.Parser.AttachObservers(context, codeObject, observers);
        }

        protected void ResolvePropertyValue(IXmlBuilderContext context, IPropertiesCodeObject codeObject, PropertyInfo prop, IEnumerable<XmlNode> content)
        {
            if (typeof(string) == prop.PropertyType)
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
            else if (typeof(ICollection).IsAssignableFrom(prop.PropertyType)
                || typeof(IEnumerable).IsAssignableFrom(prop.PropertyType)
                || (prop.PropertyType.IsGenericType && typeof(ICollection<>).IsAssignableFrom(prop.PropertyType.GetGenericTypeDefinition()))
            )
            {
                //Collection item
                IPropertyDescriptor propertyDescriptor = new ListAddPropertyDescriptor(prop);
                codeObject.Properties.Add(propertyDescriptor);
                context.Parser.ProcessContent(context, propertyDescriptor, content);
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
                        context.Parser.ProcessContent(context, propertyDescriptor, content);
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
    }
}
