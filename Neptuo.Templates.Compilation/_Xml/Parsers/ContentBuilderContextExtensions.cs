using Neptuo.ComponentModel;
using Neptuo.Templates.Compilation.CodeObjects;
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
    public static class ContentBuilderContextExtensions
    {
        /// <summary>
        /// Adds error to the parser context.
        /// </summary>
        /// <param name="context">Builder context.</param>
        /// <param name="errorInfo">Error description.</param>
        public static void AddError(this IContentBuilderContext context, IErrorInfo errorInfo)
        {
            Guard.NotNull(context, "context");
            Guard.NotNull(errorInfo, "errorInfo");
            context.ParserContext.Errors.Add(errorInfo);
        }

        /// <summary>
        /// Adds error to the parser context.
        /// </summary>
        /// <param name="context">Builder context.</param>
        /// <param name="errorText">Description of error.</param>
        public static void AddError(this IContentBuilderContext context, string errorText)
        {
            Guard.NotNull(context, "context");
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
            Guard.NotNull(context, "context");
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
            Guard.NotNull(context, "context");
            Guard.NotNull(element, "element");

            ISourceLineInfo lineInfo = element as ISourceLineInfo;
            if (lineInfo != null)
                AddError(context, lineInfo.LineIndex, lineInfo.ColumnIndex, errorText);
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
            Guard.NotNull(context, "context");
            Guard.NotNull(node, "node");

            if (node.NodeType == XmlNodeType.Element)
            {
                AddError(context, (IXmlElement)node, errorText);
                return;
            }

            ISourceLineInfo lineInfo = node as ISourceLineInfo;
            if (lineInfo != null)
                AddError(context, lineInfo.LineIndex, lineInfo.ColumnIndex, errorText);
            else
                AddError(context, String.Format("'{0}' -> {1}", node, errorText));
        }

        /// <summary>
        /// Adds error related to the <paramref name="attribute"/> described by the <paramref name="errorText"/>.
        /// </summary>
        /// <param name="context">Builder context.</param>
        /// <param name="element">The element which caused the error.</param>
        /// <param name="errorText">Text description of the error.</param>
        public static void AddError(this IContentBuilderContext context, IXmlAttribute attribute, string errorText)
        {
            Guard.NotNull(context, "context");
            Guard.NotNull(attribute, "attribute");

            ISourceLineInfo lineInfo = attribute as ISourceLineInfo;
            if (lineInfo != null)
                AddError(context, lineInfo.LineIndex, lineInfo.ColumnIndex, errorText);
            else
                AddError(context, String.Format("<{0} {1}> -> {2}", attribute.OwnerElement.Name, attribute.Name, errorText));
        }

        /// <summary>
        /// Parses content using <see cref="IParserService"/> and creates AST.
        /// </summary>
        /// <param name="context">Builder context.</param>
        /// <param name="content">Template content.</param>
        /// <returns>Parsed code object; <c>null</c> otherwise.</returns>
        public static ICodeObject ProcessContent(this IContentBuilderContext context, ISourceContent content)
        {
            return context.ParserContext.ParserService.ProcessContent(
                content,
                context.ParserContext
            );
        }

        /// <summary>
        /// Parses value using <see cref="IParserService"/> and creates AST.
        /// </summary>
        /// <param name="context">Builder context.</param>
        /// <param name="value">Template content.</param>
        /// <returns>Parsed code object; <c>null</c> otherwise.</returns>
        public static ICodeObject ProcessValue(this IContentBuilderContext context, ISourceContent value)
        {
            return context.ParserContext.ParserService.ProcessValue(
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
        public static ICodeObject ProcessValue(this IContentBuilderContext context, IXmlAttribute attribute)
        {
            return context.ParserContext.ParserService.ProcessValue(
                new DefaultSourceContent(attribute.Value, GetSourceLineInfoOrDefault(attribute)),
                context.ParserContext
            );
        }

        private static ISourceLineInfo GetSourceLineInfoOrDefault(object source)
        {
            ISourceLineInfo lineInfo = source as ISourceLineInfo;
            if (lineInfo != null)
                return lineInfo;

            return new DefaultSourceLineInfo(0, 0);
        }
    }
}