﻿using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class DefaultObserverBuilder : IObserverBuilder
    {
        public void Parse(IBuilderContext context, IComponentCodeObject codeObject, IEnumerable<XmlAttribute> attributes)
        {
            //TODO: Find observer type ...

            ObserverCodeObject observerObject = new ObserverCodeObject(observerType, livecycle);
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
                        bool result = context.ParserContext.ParserService.ProcessValue(attribute.Value, new DefaultParserServiceContext(context.ParserContext.DependencyProvider, propertyDescriptor, context.ParserContext.Errors));

                        if (!result)
                            result = context.Parser.BindPropertyDefaultValue(propertyDescriptor);

                        if (result)
                            observerObject.Properties.Add(propertyDescriptor);

                        boundAttributes.Add(attribute);
                        boundProperty = true;
                    }
                }

                if (!boundProperty)
                {
                    IPropertyDescriptor propertyDescriptor = new SetPropertyDescriptor(property.Value);
                    bool result = context.Parser.BindPropertyDefaultValue(propertyDescriptor);

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
                    context.Parser.BindAttributeCollection(context, observerObject, observerObject, attribute.LocalName, attribute.Value);
            }
        }
    }
}