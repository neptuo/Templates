﻿using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Base component builder including logic for processing XML elements as properties and inner values.
    /// </summary>
    public abstract class ComponentDescriptorBuilder : ComponentBuilder
    {
        private bool isParsing;

        protected IComponentCodeObject CodeObject { get; private set; }
        protected IComponentDescriptor ComponentDefinition { get; private set; }
        protected IPropertyInfo DefaultProperty { get; private set; }
        protected BindPropertiesContext BindContext { get; private set; }
        protected IPropertyBuilder PropertyFactory { get; private set; }

        public ComponentDescriptorBuilder(IPropertyBuilder propertyFactory)
        {
            Guard.NotNull(propertyFactory, "propertyFactory");
            PropertyFactory = propertyFactory;
        }

        public override ICodeObject TryParse(IContentBuilderContext context, IXmlElement element)
        {
            if (isParsing)
                throw Guard.Exception.InvalidOperation("ComponentDescriptorBuilder can't be reused! Create new instance for every xml tag.");

            isParsing = true;
            CodeObject = CreateCodeObject(context, element);
            ComponentDefinition = GetComponentDescriptor(context, CodeObject, element);
            DefaultProperty = ComponentDefinition.GetDefaultProperty();
            BindContext = new BindPropertiesContext(ComponentDefinition.GetProperties().ToDictionary(p => p.Name.ToLowerInvariant()));

            BindProperties(context, element);
            isParsing = false;
            return CodeObject;
        }

        protected override bool TryBindProperty(IContentBuilderContext context, string prefix, string name, ISourceContent value)
        {
            IPropertyInfo propertyInfo;
            if (BindContext.Properties.TryGetValue(name, out propertyInfo))
            {
                bool result = PropertyFactory.Parse(context, CodeObject, propertyInfo, value);
                if (result)
                {
                    BindContext.BoundProperies.Add(name);
                    return true;
                }

                // TODO: All properties through IPropertyBuilder.
                ICodeObject codeObject = context.ProcessValue(value);
                if (codeObject != null)
                {
                    IPropertyDescriptor propertyDescriptor = CreatePropertyDescriptor(propertyInfo);
                    propertyDescriptor.SetValue(codeObject);
                    CodeObject.Properties.Add(propertyDescriptor);
                    BindContext.BoundProperies.Add(name);
                    return true;
                }
            }

            return false;
        }

        protected override bool TryBindProperty(IContentBuilderContext context, string prefix, string name, IEnumerable<IXmlNode> value)
        {
            IPropertyInfo propertyInfo;
            if (!BindContext.BoundProperies.Contains(name) && BindContext.Properties.TryGetValue(name, out propertyInfo))
            {
                ResolvePropertyValue(context, CodeObject, propertyInfo, value);
                BindContext.BoundProperies.Add(name);
                return true;
            }

            return false;
        }

        protected override bool ProcessUnboundAttributes(IContentBuilderContext context, IEnumerable<IXmlAttribute> unboundAttributes)
        {
            bool result = true;
            List<IXmlAttribute> usedAttributes = new List<IXmlAttribute>();
            ObserverCollection observers = new ObserverCollection();
            foreach (IXmlAttribute attribute in unboundAttributes)
            {
                bool boundAttribute = false;

                if (attribute.Prefix.ToLowerInvariant() == "xmlns")
                    boundAttribute = true;

                if (!boundAttribute)
                {
                    //TODO: Observers...
                    //IObserverRegistration observer = context.BuilderRegistry.GetObserverBuilder(attribute.Prefix, attribute.LocalName);
                    //if (observer != null)
                    //{
                    //    if (observer.Scope == ObserverBuilderScope.PerElement && observers.ContainsKey(observer))
                    //        observers[observer].Attributes.Add(attribute);
                    //    else
                    //        observers.Add(observer, attribute);

                    //    boundAttribute = true;
                    //}
                }

                if (!boundAttribute)
                {
                    if (!ProcessUnboundAttribute(context, attribute))
                        result = false;
                }
            }

            context.Parser.AttachObservers(context, CodeObject, observers);
            return result;
        }

        protected override bool ProcessUnboundNodes(IContentBuilderContext context, IEnumerable<IXmlNode> unboundNodes)
        {
            if (unboundNodes.Any())
            {
                // Bind content elements
                if (DefaultProperty != null && !BindContext.BoundProperies.Contains(DefaultProperty.Name.ToLowerInvariant()))
                    return ResolvePropertyValue(context, CodeObject, DefaultProperty, unboundNodes);

                return false;
            }

            return true;
        }

        /// <summary>
        /// Should create code object for this component.
        /// </summary>
        protected abstract IComponentCodeObject CreateCodeObject(IContentBuilderContext context, IXmlElement element);

        /// <summary>
        /// Gets current component definition.
        /// </summary>
        protected abstract IComponentDescriptor GetComponentDescriptor(IContentBuilderContext context, IComponentCodeObject codeObject, IXmlElement element);

        /// <summary>
        /// Should create property descriptor for <paramref name="propertyInfo"/>.
        /// </summary>
        protected abstract IPropertyDescriptor CreatePropertyDescriptor(IPropertyInfo propertyInfo);

        /// <summary>
        /// Some magic logic to create right proproperty descriptors for right property types.
        /// </summary>
        protected virtual bool ResolvePropertyValue(IContentBuilderContext context, IPropertiesCodeObject codeObject, IPropertyInfo propertyInfo, IEnumerable<IXmlNode> content)
        {
            bool result = PropertyFactory.Parse(context, codeObject, propertyInfo, content);
            if (result)
                return true;

            // TODO: All properties through IPropertyBuilder.
            if (typeof(string) == propertyInfo.Type)
            {
                //Get string and add as plain value
                StringBuilder contentValue = new StringBuilder();
                foreach (IXmlNode node in content)
                    contentValue.Append(node.OuterXml);

                IPropertyDescriptor propertyDescriptor = CreatePropertyDescriptor(propertyInfo);
                propertyDescriptor.SetValue(new PlainValueCodeObject(contentValue.ToString()));
                codeObject.Properties.Add(propertyDescriptor);
                return true;
            }
            else if (typeof(IEnumerable).IsAssignableFrom(propertyInfo.Type))
            {
                //Collection item
                IPropertyDescriptor propertyDescriptor = CreatePropertyDescriptor(propertyInfo);
                codeObject.Properties.Add(propertyDescriptor);

                foreach (IXmlNode node in content)
                {
                    ICodeObject valueObject = context.Parser.TryProcessNode(context, node);
                    propertyDescriptor.SetValue(valueObject);
                }

                return true;
            }
            else
            {
                // Count elements
                IEnumerable<IXmlElement> elements = content.OfType<IXmlElement>();
                if (elements.Any())
                {
                    // One IXmlElement is ok
                    if (elements.Count() == 1)
                    {
                        IPropertyDescriptor propertyDescriptor = CreatePropertyDescriptor(propertyInfo);
                        codeObject.Properties.Add(propertyDescriptor);

                        foreach (IXmlElement node in elements)
                        {
                            ICodeObject valueObject = context.Parser.TryProcessNode(context, node);
                            propertyDescriptor.SetValue(valueObject);
                        }
                        return true;
                    }
                    else
                    {
                        IXmlElement element = elements.ElementAt(1);
                        context.AddError(element, "Property supports only single value.");
                        return false;
                    }
                }
                else
                {
                    //Get string and add as plain value
                    StringBuilder contentValue = new StringBuilder();
                    foreach (IXmlNode node in content)
                        contentValue.Append(node.OuterXml);

                    IPropertyDescriptor propertyDescriptor = CreatePropertyDescriptor(propertyInfo);
                    propertyDescriptor.SetValue(new PlainValueCodeObject(contentValue.ToString()));
                    codeObject.Properties.Add(propertyDescriptor);
                    return true;
                }
            }
        }
    }
}