using Neptuo.Models.Features;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers.Descriptors;
using Neptuo.Templates.Compilation.Parsers.Descriptors.Features;
using Neptuo.Templates.Compilation.Parsers.Normalization;
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
    /// 
    /// </summary>
    public abstract class ComponentDescriptorBuilder : ComponentBuilder
    {
        public override IEnumerable<ICodeObject> TryParse(IContentBuilderContext context, IXmlElement element)
        {
            ICodeObject codeObject = CreateCodeObject(context, element);
            IComponentDescriptor componentDefinition = GetComponentDescriptor(context, codeObject, element);
            IFieldDescriptor defaultField = componentDefinition.With<IDefaultFieldEnumerator>().FirstOrDefault();
            BindContentPropertiesContext bindContext = new BindContentPropertiesContext(componentDefinition, context.Registry.WithPropertyNormalizer());
            context.CodeObject(codeObject);
            context.ComponentDescriptor(componentDefinition);
            context.BindPropertiesContext(bindContext);
            context.DefaultField(defaultField);

            BindProperties(context, element);
            return new List<ICodeObject> { codeObject };
        }

        protected override bool TryBindProperty(IContentBuilderContext context, string prefix, string name, ISourceContent value)
        {
            IFieldDescriptor fieldDescriptor;
            if (context.BindPropertiesContext().Fields.TryGetValue(name, out fieldDescriptor))
            {
                IEnumerable<ICodeProperty> codeProperties = context.TryProcessProperty(fieldDescriptor, value);
                if (codeProperties != null)
                {
                    IFieldCollectionCodeObject fields = context.CodeObjectAsProperties();
                    foreach (ICodeProperty codeProperty in codeProperties)
                        fields.AddProperty(codeProperty);

                    context.BindPropertiesContext().BoundProperties.Add(name);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Tries to find property named <paramref name="prefix"/> + <paramref name="name"/> and delegates parsing it's value to property factory.
        /// </summary>
        protected override bool TryBindProperty(IContentBuilderContext context, string prefix, string name, IEnumerable<IXmlNode> value)
        {
            IFieldDescriptor fieldDescriptor;
            if (!context.BindPropertiesContext().BoundProperties.Contains(name) && context.BindPropertiesContext().Fields.TryGetValue(name, out fieldDescriptor))
            {
                IEnumerable<ICodeProperty> codeProperties = context.TryProcessProperty(fieldDescriptor, value);
                if (codeProperties != null)
                {
                    IFieldCollectionCodeObject fields = context.CodeObjectAsProperties();
                    foreach (ICodeProperty codeProperty in codeProperties)
                        fields.AddProperty(codeProperty);

                    context.BindPropertiesContext().BoundProperties.Add(name);
                    context.BindPropertiesContext().IsBoundFromContent = true;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Skips xmlns definitions, other attributes tries to parse as observers; otherwise marks those as errors.
        /// </summary>
        protected override bool ProcessUnboundAttributes(IContentBuilderContext context, IEnumerable<IXmlAttribute> unboundAttributes)
        {
            bool result = true;
            List<IXmlAttribute> usedAttributes = new List<IXmlAttribute>();
            INameNormalizer nameNormalizer = context.Registry.WithPropertyNormalizer();
            foreach (IXmlAttribute attribute in unboundAttributes)
            {
                bool boundAttribute = false;

                // TODO: Process xmlns registration.
                if (nameNormalizer.PreparePrefix(attribute.Prefix) == "xmlns")
                    boundAttribute = true;

                // Call base if attribute was not bound.
                if (!boundAttribute && !ProcessUnboundAttribute(context, attribute))
                    result = false;
            }

            return result;
        }

        /// <summary>
        /// Tries to bind default property from <paramref name="unboundNodes"/>.
        /// </summary>
        protected override bool ProcessUnboundNodes(IContentBuilderContext context, IEnumerable<IXmlNode> unboundNodes)
        {
            if (unboundNodes.Any())
            {
                // If at least one property was bound from content element, default property is not supported.
                if (context.BindPropertiesContext().IsBoundFromContent)
                {
                    IXmlNode node = FindFirstSignificantNode(unboundNodes);
                    if (node != null)
                    {
                        context.AddError(node, "If at least one property was bound from content element, default property is not supported.");
                        return false;
                    }

                    return true;
                }

                // Bind content elements
                IFieldDescriptor defaultField = context.DefaultField();
                INameNormalizer nameNormalizer = context.Registry.WithPropertyNormalizer();
                if (defaultField != null && !context.BindPropertiesContext().BoundProperties.Contains(nameNormalizer.PrepareName(defaultField.Name)))
                {
                    IEnumerable<ICodeProperty> codeProperties = context.TryProcessProperty(defaultField, unboundNodes);
                    if (codeProperties != null)
                    {
                        IFieldCollectionCodeObject fields = context.CodeObjectAsProperties();
                        foreach (ICodeProperty codeProperty in codeProperties)
                            fields.AddProperty(codeProperty);

                        context.BindPropertiesContext().BoundProperties.Add(nameNormalizer.PrepareName(defaultField.Name));
                        return true;
                    }
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns first not white space node from <paramref name="nodes"/>; otherwise <c>null</c>;
        /// </summary>
        /// <param name="nodes">Enumeration of nodes to search.</param>
        /// <returns>First not white space node from <paramref name="nodes"/>; otherwise <c>null</c>;</returns>
        protected IXmlNode FindFirstSignificantNode(IEnumerable<IXmlNode> nodes)
        {
            IXmlNode node = nodes.FirstOrDefault(n => n.NodeType == XmlNodeType.Element);
            if (node == null)
                node = nodes.OfType<IXmlText>().FirstOrDefault(t => !String.IsNullOrEmpty(t.Text) && !String.IsNullOrWhiteSpace(t.Text));

            return node;
        }

        /// <summary>
        /// Should create code object for this component.
        /// </summary>
        protected abstract IComponentCodeObject CreateCodeObject(IContentBuilderContext context, IXmlElement element);

        /// <summary>
        /// Gets current component definition.
        /// </summary>
        protected abstract IComponentDescriptor GetComponentDescriptor(IContentBuilderContext context, ICodeObject codeObject, IXmlElement element);
    }
}