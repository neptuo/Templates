using Neptuo.Compilers.Errors;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers.Descriptors;
using Neptuo.Text.Positions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Some common extensions for <see cref="IContentBuilderContext"/>.
    /// </summary>
    public static class _ContentBuilderContextExtensions
    {
        /// <summary>
        /// Adds error to the parser context.
        /// </summary>
        /// <param name="context">Builder context.</param>
        /// <param name="errorInfo">Error description.</param>
        public static void AddError(this IContentBuilderContext context, IErrorInfo errorInfo)
        {
            Ensure.NotNull(context, "context");
            Ensure.NotNull(errorInfo, "errorInfo");
            context.ParserContext.Errors.Add(errorInfo);
        }

        /// <summary>
        /// Adds error to the parser context.
        /// </summary>
        /// <param name="context">Builder context.</param>
        /// <param name="errorText">Description of error.</param>
        public static void AddError(this IContentBuilderContext context, string errorText)
        {
            Ensure.NotNull(context, "context");
            context.AddError(new ErrorInfo(1, 1, errorText));
        }

        /// <summary>
        /// Adds error to the parser context.
        /// </summary>
        /// <param name="context">Builder context.</param>
        /// <param name="line">Source file line number.</param>
        /// <param name="column">Source file line column number.</param>
        /// <param name="errorText">Description of error.</param>
        public static void AddError(this IContentBuilderContext context, int line, int column, string errorText)
        {
            Ensure.NotNull(context, "context");
            context.AddError(new ErrorInfo(line, column, errorText));
        }

        /// <summary>
        /// Adds error related to the <paramref name="element"/> described by the <paramref name="errorText"/>.
        /// </summary>
        /// <param name="context">Builder context.</param>
        /// <param name="element">The element which caused the error.</param>
        /// <param name="errorText">Text description of the error.</param>
        public static void AddError(this IContentBuilderContext context, IXmlElement element, string errorText)
        {
            Ensure.NotNull(context, "context");
            Ensure.NotNull(element, "element");

            IDocumentPoint point = element as IDocumentPoint;
            if (point != null)
                AddError(context, point.LineIndex, point.ColumnIndex, errorText);
            else
                AddError(context, String.Format("<{0}> -> {1}", element.Name, errorText));
        }

        /// <summary>
        /// Adds error related to the <paramref name="node"/> described by the <paramref name="errorText"/>.
        /// </summary>
        /// <param name="context">Builder context.</param>
        /// <param name="node">The element which caused the error.</param>
        /// <param name="errorText">Text description of the error.</param>
        public static void AddError(this IContentBuilderContext context, IXmlNode node, string errorText)
        {
            Ensure.NotNull(context, "context");
            Ensure.NotNull(node, "node");

            if (node.NodeType == XmlNodeType.Element)
            {
                AddError(context, (IXmlElement)node, errorText);
                return;
            }

            IDocumentPoint point = node as IDocumentPoint;
            if (point != null)
                AddError(context, point.LineIndex, point.ColumnIndex, errorText);
            else
                AddError(context, String.Format("'{0}' -> {1}", node, errorText));
        }

        /// <summary>
        /// Adds error related to the <paramref name="attribute"/> described by the <paramref name="errorText"/>.
        /// </summary>
        /// <param name="context">Builder context.</param>
        /// <param name="attribute">The attribute which caused the error.</param>
        /// <param name="errorText">Text description of the error.</param>
        public static void AddError(this IContentBuilderContext context, IXmlAttribute attribute, string errorText)
        {
            Ensure.NotNull(context, "context");
            Ensure.NotNull(attribute, "attribute");

            IDocumentPoint point = attribute as IDocumentPoint;
            if (point != null)
                AddError(context, point.LineIndex, point.ColumnIndex, errorText);
            else
                AddError(context, String.Format("<{0} {1}> -> {2}", attribute.OwnerElement.Name, attribute.Name, errorText));
        }

        /// <summary>
        /// Parses content using <see cref="IParserService"/> and creates AST.
        /// </summary>
        /// <param name="context">Builder context.</param>
        /// <param name="content">Template content.</param>
        /// <returns>Parsed code object; <c>null</c> otherwise.</returns>
        public static ICodeObject TryProcessContent(this IContentBuilderContext context, ISourceContent content)
        {
            Ensure.NotNull(context, "context");
            return context.ParserContext.ParserService.ProcessContent(
                context.ParserContext.Name,
                content,
                context.ParserContext
            );
        }

        /// <summary>
        /// Parses content using <see cref="TextXmlContentParser"/> and creates AST for <paramref name="nodes"/>.
        /// </summary>
        /// <param name="context">Builder context.</param>
        /// <param name="nodes">Enumeration of XML nodes to process.</param>
        /// <returns>Parser code objects; <c>null</c> otherwise.</returns>
        public static IEnumerable<ICodeObject> TryProcessContentNodes(this IContentBuilderContext context, IEnumerable<IXmlNode> nodes)
        {
            Ensure.NotNull(context, "context");
            Ensure.NotNull(nodes, "nodes");

            CodeObjectCollection result = new CodeObjectCollection();
            foreach (IXmlNode node in nodes)
            {
                IEnumerable<ICodeObject> values = context.Parser.TryProcessNode(context, node);
                if (values == null)
                    return null;

                result.AddRange(values);
            }

            return result;
        }

        /// <summary>
        /// Parses value using <see cref="IParserService"/> and creates AST.
        /// </summary>
        /// <param name="context">Builder context.</param>
        /// <param name="value">Template content.</param>
        /// <returns>Parsed code object; <c>null</c> otherwise.</returns>
        public static ICodeObject TryProcessValue(this IContentBuilderContext context, ISourceContent value)
        {
            Ensure.NotNull(context, "context");

            return context.ParserContext.ParserService.ProcessValue(
                context.ParserContext.Name,
                value,
                context.ParserContext
            );
        }

        /// <summary>
        /// Parses value using <see cref="IParserService"/> and creates AST.
        /// </summary>
        /// <param name="context">Builder context.</param>
        /// <param name="attribute">Attribute which to process.</param>
        /// <returns>Parsed code object; <c>null</c> otherwise.</returns>
        public static ICodeObject TryProcessValue(this IContentBuilderContext context, IXmlAttribute attribute)
        {
            return context.ParserContext.ParserService.ProcessValue(
                context.ParserContext.Name,
                new DefaultSourceContent(attribute.Value, GetDocumentPointOrDefault(attribute)),
                context.ParserContext
            );
        }

        private static IDocumentPoint GetDocumentPointOrDefault(object source)
        {
            IDocumentPoint point = source as IDocumentPoint;
            if (point != null)
                return point;

            return new DefaultSourceLineInfo(0, 0);
        }

        /// <summary>
        /// Parses property value.
        /// </summary>
        /// <param name="context">Builder context.</param>
        /// <param name="fieldDescriptor">Property to build.</param>
        /// <param name="value">Value.</param>
        /// <returns>Parsed property descriptors.</returns>
        public static IEnumerable<ICodeProperty> TryProcessProperty(this IContentBuilderContext context, IFieldDescriptor fieldDescriptor, ISourceContent value)
        {
            Ensure.NotNull(context, "context");
            return context.Registry.XWithPropertyBuilder().TryParse(new ContentPropertyBuilderContext(context, fieldDescriptor), value);
        }

        /// <summary>
        /// Parses property value.
        /// </summary>
        /// <param name="context">Builder context.</param>
        /// <param name="fieldDescriptor">Property to build.</param>
        /// <param name="content">Content.</param>
        /// <returns>Parsed property descriptors.</returns>
        public static IEnumerable<ICodeProperty> TryProcessProperty(this IContentBuilderContext context, IFieldDescriptor fieldDescriptor, IEnumerable<IXmlNode> content)
        {
            Ensure.NotNull(context, "context");
            return context.Registry.WithContentPropertyBuilder().TryParse(new ContentPropertyBuilderContext(context, fieldDescriptor), content);
        }
    }
}