using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Some magic logic to create right proproperty descriptors for right property types.
    /// </summary>
    public abstract class DefaultPropertyBuilder : IContentPropertyBuilder, IPropertyBuilder
    {
        /// <summary>
        /// Should create property descriptor for <paramref name="propertyInfo"/>.
        /// </summary>
        protected abstract ICodeProperty CreateCodeProperty(IPropertyInfo propertyInfo);

        public IEnumerable<ICodeProperty> TryParse(IContentPropertyBuilderContext context, IEnumerable<IXmlNode> content)
        {
            if (typeof(string) == context.PropertyInfo.Type)
            {
                //Get string and add as plain value
                StringBuilder contentValue = new StringBuilder();
                foreach (IXmlNode node in content)
                    contentValue.Append(node.OuterXml);

                ICodeProperty codeProperty = CreateCodeProperty(context.PropertyInfo);
                codeProperty.SetValue(new PlainValueCodeObject(contentValue.ToString()));
                return new CodePropertyCollection(codeProperty);
            }
            else if (typeof(IEnumerable).IsAssignableFrom(context.PropertyInfo.Type))
            {
                //Collection item
                ICodeProperty codeProperty = CreateCodeProperty(context.PropertyInfo);
                bool isTextAssignable = codeProperty.Type.IsAssignableFrom(typeof(string));
                foreach (IXmlNode node in content)
                {
                    if (!isTextAssignable && node.NodeType == XmlNodeType.Text)
                    {
                        string textValue = ((IXmlText)node).Text;
                        if (String.IsNullOrEmpty(textValue) || String.IsNullOrWhiteSpace(textValue))
                            continue;
                    }

                    IEnumerable<ICodeObject> values = context.Parser.TryProcessNode(context.BuilderContext, node);
                    if (values != null)
                        codeProperty.SetRangeValue(values);
                }

                return new CodePropertyCollection(codeProperty);
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
                        ICodeProperty codeProperty = CreateCodeProperty(context.PropertyInfo);
                        foreach (IXmlElement node in elements)
                        {
                            IEnumerable<ICodeObject> values = context.Parser.TryProcessNode(context.BuilderContext, node);
                            if (values != null)
                            {
                                //TODO: Here MUST be only one value!
                                codeProperty.SetRangeValue(values);
                                return new CodePropertyCollection().Add(codeProperty);
                            }
                        }
                        return null;
                    }
                    else
                    {
                        IXmlElement element = elements.ElementAt(1);
                        context.AddError(element, "Property supports only single value.");
                        return null;
                    }
                }
                else
                {
                    //Get string and add as plain value
                    StringBuilder contentValue = new StringBuilder();
                    foreach (IXmlNode node in content)
                        contentValue.Append(node.OuterXml);

                    ICodeProperty codeProperty = CreateCodeProperty(context.PropertyInfo);
                    codeProperty.SetValue(new PlainValueCodeObject(contentValue.ToString()));
                    return new CodePropertyCollection(codeProperty);
                }
            }
        }

        public IEnumerable<ICodeProperty> TryParse(IPropertyBuilderContext context, ISourceContent value)
        {
            ICodeObject propertyValue = context.ParserService.ProcessValue(context.Name, value, context);
            if (propertyValue != null)
            {
                ICodeProperty codeProperty = CreateCodeProperty(context.PropertyInfo);
                codeProperty.SetValue(propertyValue);
                return new CodePropertyCollection(codeProperty);
            }

            return null;
        }
    }
}
