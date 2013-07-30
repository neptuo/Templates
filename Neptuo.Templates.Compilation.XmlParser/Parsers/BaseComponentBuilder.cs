using Neptuo.Templates.Compilation.CodeObjects;
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
        public void Parse(IBuilderContext context, XmlElement element)
        {
            IComponentCodeObject codeObject = CreateCodeObject(context, element);
            BindProperties(context, codeObject, element);
            AppendToParent(context, codeObject);
        }

        protected virtual void AppendToParent(IBuilderContext context, IComponentCodeObject codeObject)
        {
            context.Parent.SetValue(codeObject);
        }

        protected abstract IComponentCodeObject CreateCodeObject(IBuilderContext context, XmlElement element);

        protected abstract IComponentDefinition GetComponentDefinition(IBuilderContext context, IComponentCodeObject codeObject, XmlElement element);

        protected abstract IPropertyDescriptor CreateSetPropertyDescriptor(IPropertyInfo propertyInfo);
        protected abstract IPropertyDescriptor CreateListAddPropertyDescriptor(IPropertyInfo propertyInfo);

        protected virtual void BindProperties(IBuilderContext context, IComponentCodeObject codeObject, XmlElement element)
        {
            IComponentDefinition componentDefinition = GetComponentDefinition(context, codeObject, element);

            HashSet<string> boundProperies = new HashSet<string>();
            IPropertyInfo defaultProperty = componentDefinition.GetDefaultProperty();
            List<XmlAttribute> boundAttributes = new List<XmlAttribute>();

            foreach (IPropertyInfo propertyInfo in componentDefinition.GetProperties())
            {
                bool bound = false;
                string propertyName = propertyInfo.Name.ToLowerInvariant();
                foreach (XmlAttribute attribute in element.Attributes)
                {
                    if (propertyName == attribute.Name.ToLowerInvariant())
                    {
                        IPropertyDescriptor propertyDescriptor = CreateSetPropertyDescriptor(propertyInfo);
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
                    ResolvePropertyValue(context, codeObject, propertyInfo, child.ChildNodes.ToEnumerable());
                    boundProperies.Add(propertyName);
                    bound = true;
                }

                //if (!bound && item.Value != defaultProperty)
                //{
                //    IPropertyDescriptor propertyDescriptor = new SetPropertyDescriptor(item.Value);
                //    if (context.Parser.BindPropertyDefaultValue(propertyDescriptor))
                //    {
                //        codeObject.Properties.Add(propertyDescriptor);
                //        boundProperies.Add(propertyName);
                //        bound = true;
                //    }
                //}
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
                    IPropertyDescriptor propertyDescriptor = CreateSetPropertyDescriptor(defaultProperty);
                    if (context.Parser.BindPropertyDefaultValue(propertyDescriptor))
                        codeObject.Properties.Add(propertyDescriptor);
                }
            }
        }

        protected virtual void ProcessUnboundAttributes(IBuilderContext context, IComponentCodeObject codeObject, List<XmlAttribute> unboundAttributes)
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

        protected abstract void ProcessUnboundAttribute(IBuilderContext context, IComponentCodeObject codeObject, XmlAttribute unboundAttribute);

        protected virtual void ResolvePropertyValue(IBuilderContext context, IPropertiesCodeObject codeObject, IPropertyInfo propertyInfo, IEnumerable<XmlNode> content)
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
