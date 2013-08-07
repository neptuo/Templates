﻿using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Xml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Neptuo.Templates.Compilation.Parsers
{
    public abstract class BaseComponentBuilder : IComponentBuilder
    {
        public void Parse(IContentBuilderContext context, XmlElement element)
        {
            IComponentCodeObject codeObject = CreateCodeObject(context, element);
            BindProperties(context, codeObject, element);
            AppendToParent(context, codeObject);
        }

        protected virtual void AppendToParent(IContentBuilderContext context, IComponentCodeObject codeObject)
        {
            context.Parent.SetValue(codeObject);
        }

        protected abstract IComponentCodeObject CreateCodeObject(IContentBuilderContext context, XmlElement element);
        protected abstract IComponentDefinition GetComponentDefinition(IContentBuilderContext context, IComponentCodeObject codeObject, XmlElement element);

        protected abstract IPropertyDescriptor CreateSetPropertyDescriptor(IPropertyInfo propertyInfo);
        protected abstract IPropertyDescriptor CreateListAddPropertyDescriptor(IPropertyInfo propertyInfo);

        protected virtual void BindProperties(IContentBuilderContext context, IComponentCodeObject codeObject, XmlElement element)
        {
            IComponentDefinition componentDefinition = GetComponentDefinition(context, codeObject, element);
            IPropertyInfo defaultProperty = componentDefinition.GetDefaultProperty();
            BindPropertiesContext bindContext = new BindPropertiesContext(componentDefinition.GetProperties().ToDictionary(p => p.Name.ToLowerInvariant()));

            // Bind attributes
            BindAttributes(context, bindContext, codeObject, element.Attributes);
            
            // Bind inner elements
            BindInnerElements(context, bindContext, codeObject, element.ChildNodes);

            // Process unbound attributes
            ProcessUnboundAttributes(context, codeObject, bindContext.UnboundAttributes);

            // Bind content elements
            BindContentElements(context, bindContext, codeObject, defaultProperty, element.ChildNodes);
        }

        protected virtual void BindAttributes(IContentBuilderContext context, BindPropertiesContext bindContext, IComponentCodeObject codeObject, XmlAttributeCollection attributes)
        {
            // Bind attributes
            foreach (XmlAttribute attribute in attributes)
            {
                string attributeName = attribute.Name.ToLowerInvariant();
                IPropertyInfo propertyInfo;
                if (bindContext.Properties.TryGetValue(attributeName, out propertyInfo))
                {
                    IPropertyDescriptor propertyDescriptor = CreateSetPropertyDescriptor(propertyInfo);
                    bool result = context.ParserContext.ParserService.ProcessValue(
                        attribute.Value,
                        new DefaultParserServiceContext(context.ParserContext.DependencyProvider, propertyDescriptor, context.ParserContext.Errors)
                    );

                    if (result)
                    {
                        codeObject.Properties.Add(propertyDescriptor);
                        bindContext.BoundProperies.Add(attributeName);
                    }
                    else
                    {
                        bindContext.UnboundAttributes.Add(attribute);
                    }
                }
                else
                {
                    bindContext.UnboundAttributes.Add(attribute);
                }
            }
        }

        protected virtual void BindInnerElements(IContentBuilderContext context, BindPropertiesContext bindContext, IComponentCodeObject codeObject, XmlNodeList childNodes)
        {
            // Bind inner elements
            foreach (XmlNode childNode in childNodes)
            {
                string childName = childNode.Name.ToLowerInvariant();
                IPropertyInfo propertyInfo;
                if (!bindContext.BoundProperies.Contains(childName) && bindContext.Properties.TryGetValue(childName, out propertyInfo))
                {
                    ResolvePropertyValue(context, codeObject, propertyInfo, childNode.ChildNodes.ToEnumerable());
                    bindContext.BoundProperies.Add(childName);
                }
            }
        }

        protected virtual void BindContentElements(IContentBuilderContext context, BindPropertiesContext bindContext, IComponentCodeObject codeObject, IPropertyInfo defaultProperty, XmlNodeList childNodes)
        {
            // Bind content elements
            if (defaultProperty != null && !bindContext.BoundProperies.Contains(defaultProperty.Name.ToLowerInvariant()))
            {
                IEnumerable<XmlNode> defaultChildNodes = XmlContentParser.Utils.FindNotUsedChildNodes(childNodes, bindContext.BoundProperies);
                if (defaultChildNodes.Any())
                    ResolvePropertyValue(context, codeObject, defaultProperty, defaultChildNodes);
            }
        }

        protected virtual void ProcessUnboundAttributes(IContentBuilderContext context, IComponentCodeObject codeObject, List<XmlAttribute> unboundAttributes)
        {
            List<XmlAttribute> usedAttributes = new List<XmlAttribute>();
            XmlContentParser.ObserverList observers = new XmlContentParser.ObserverList();
            foreach (XmlAttribute attribute in unboundAttributes)
            {
                bool boundAttribute = false;

                if (attribute.Prefix.ToLowerInvariant() == "xmlns")
                    boundAttribute = true;

                if (!boundAttribute)
                {
                    IObserverRegistration observer = context.BuilderRegistry.GetObserverBuilder(attribute.Prefix, attribute.LocalName);
                    if (observer != null)
                    {
                        if (observer.Scope == ObserverBuilderScope.PerElement && observers.ContainsKey(observer))
                            observers[observer].Attributes.Add(attribute);
                        else
                            observers.Add(new XmlContentParser.ParsedObserver(observer, attribute));

                        boundAttribute = true;
                    }
                }

                if (!boundAttribute)
                    ProcessUnboundAttribute(context, codeObject, attribute);
            }

            context.Parser.AttachObservers(context, codeObject, observers);
        }

        protected abstract void ProcessUnboundAttribute(IContentBuilderContext context, IComponentCodeObject codeObject, XmlAttribute unboundAttribute);

        protected virtual void ResolvePropertyValue(IContentBuilderContext context, IPropertiesCodeObject codeObject, IPropertyInfo propertyInfo, IEnumerable<XmlNode> content)
        {
            if (typeof(string) == propertyInfo.Type)
            {
                //Get string and add as plain value
                StringBuilder contentValue = new StringBuilder();
                foreach (XmlNode node in content)
                    contentValue.Append(node.OuterXml);

                IPropertyDescriptor propertyDescriptor = CreateSetPropertyDescriptor(propertyInfo);
                propertyDescriptor.SetValue(new PlainValueCodeObject(contentValue.ToString()));
                codeObject.Properties.Add(propertyDescriptor);
            }
            else if (typeof(ICollection).IsAssignableFrom(propertyInfo.Type)
                || typeof(IEnumerable).IsAssignableFrom(propertyInfo.Type)
                || (propertyInfo.Type.IsGenericType && typeof(ICollection<>).IsAssignableFrom(propertyInfo.Type.GetGenericTypeDefinition()))
            )
            {
                //Collection item
                IPropertyDescriptor propertyDescriptor = CreateListAddPropertyDescriptor(propertyInfo);
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
                        IPropertyDescriptor propertyDescriptor = CreateSetPropertyDescriptor(propertyInfo);
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

                    IPropertyDescriptor propertyDescriptor = CreateSetPropertyDescriptor(propertyInfo);
                    propertyDescriptor.SetValue(new PlainValueCodeObject(contentValue.ToString()));
                    codeObject.Properties.Add(propertyDescriptor);
                }
            }
        }
    }
}
