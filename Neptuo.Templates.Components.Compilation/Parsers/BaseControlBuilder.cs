using Neptuo.Linq.Expressions;
using Neptuo.Xml;
using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Neptuo.Templates.Compilation.Parsers
{
    public abstract class BaseControlBuilder : BaseComponentBuilder
    {
        protected abstract Type GetControlType(XmlElement element);

        protected override IComponentCodeObject CreateCodeObject(IBuilderContext context, XmlElement element)
        {
            return new ControlCodeObject(GetControlType(element));
        }

        protected override void BindProperties(IBuilderContext context, IComponentCodeObject codeObject, XmlElement element)
        {
            IComponentDefinition componentDefinition = new TypeComponentDefinition(GetControlType(element));

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
                        IPropertyDescriptor propertyDescriptor = new SetPropertyDescriptor(propertyInfo.Name);
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
                    IPropertyDescriptor propertyDescriptor = new SetPropertyDescriptor(defaultProperty.Name);
                    if (context.Parser.BindPropertyDefaultValue(propertyDescriptor))
                        codeObject.Properties.Add(propertyDescriptor);
                }
            }
        }

        protected override void ProcessUnboundAttributes(IBuilderContext context, IComponentCodeObject codeObject, List<XmlAttribute> unboundAttributes)
        {
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

                if (!boundAttribute && typeof(IAttributeCollection).IsAssignableFrom(codeObject.Type))
                    boundAttribute = BindAttributeCollection(context, codeObject, codeObject, attribute.LocalName, attribute.Value);
            }

            context.Parser.AttachObservers(context, codeObject, observers);
        }

        protected virtual void ResolvePropertyValue(
            IBuilderContext context, 
            IPropertiesCodeObject codeObject, 
            IPropertyInfo propertyInfo, 
            IEnumerable<XmlNode> content)
        {
            if (typeof(string) == propertyInfo.Type)
            {
                //Get string and add as plain value
                StringBuilder contentValue = new StringBuilder();
                foreach (XmlNode node in content)
                    contentValue.Append(node.OuterXml);

                codeObject.Properties.Add(
                    new SetPropertyDescriptor(
                        propertyInfo.Name,
                        new PlainValueCodeObject(contentValue.ToString())
                    )
                );
            }
            else if (typeof(ICollection).IsAssignableFrom(propertyInfo.Type)
                || typeof(IEnumerable).IsAssignableFrom(propertyInfo.Type)
                || (propertyInfo.Type.IsGenericType && typeof(ICollection<>).IsAssignableFrom(propertyInfo.Type.GetGenericTypeDefinition()))
            )
            {
                //Collection item
                IPropertyDescriptor propertyDescriptor = new ListAddPropertyDescriptor(propertyInfo.Name);
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
                        IPropertyDescriptor propertyDescriptor = new SetPropertyDescriptor(propertyInfo.Name);
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
                            propertyInfo.Name,
                            new PlainValueCodeObject(contentValue.ToString())
                        )
                    );
                }
            }
        }

        public static bool BindAttributeCollection(IBuilderContext context, ITypeCodeObject typeCodeObject, IPropertiesCodeObject propertiesCodeObject, string name, string value)
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

    }
}
