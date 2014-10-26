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
        /// Adds error to parser context in context.
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
        /// Adds error to parser context in context.
        /// </summary>
        /// <param name="context">Builder context.</param>
        /// <param name="errorText">Description of error.</param>
        public static void AddError(this IContentBuilderContext context, string errorText)
        {
            Guard.NotNull(context, "context");
            context.AddError(new ErrorInfo(1, 1, errorText));
        }

        /// <summary>
        /// Adds error to parser context in context.
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
        /// Parses content using <see cref="IParserService"/> and creates AST.
        /// </summary>
        /// <param name="context">Builder context.</param>
        /// <param name="content">Template content.</param>
        /// <returns>Parsed code object; <c>null</c> otherwise.</returns>
        public static ICodeObject ProcessContent(this IContentBuilderContext context, string content)
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
        public static ICodeObject ProcessValue(this IContentBuilderContext context, string value)
        {
            return context.ParserContext.ParserService.ProcessValue(
                value,
                context.ParserContext
            );
        }
    }
}