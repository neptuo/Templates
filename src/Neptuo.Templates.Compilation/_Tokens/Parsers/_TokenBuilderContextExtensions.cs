using Neptuo.ComponentModel;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Some common extensions for <see cref="ITokenBuilderContext"/>.
    /// </summary>
    public static class _TokenBuilderContextExtensions
    {
        /// <summary>
        /// Adds error to parser context in context.
        /// </summary>
        /// <param name="context">Builder context.</param>
        /// <param name="errorInfo">Error description.</param>
        public static void AddError(this ITokenBuilderContext context, IErrorInfo errorInfo)
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
        public static void AddError(this ITokenBuilderContext context, string errorText)
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
        public static void AddError(this ITokenBuilderContext context, int line, int column, string errorText)
        {
            Guard.NotNull(context, "context");
            context.AddError(new ErrorInfo(line, column, errorText));
        }

        /// <summary>
        /// Adds error related to the <paramref name="token"/> described by the <paramref name="errorText"/>.
        /// </summary>
        /// <param name="context">Builder context.</param>
        /// <param name="token">The token which caused the error.</param>
        /// <param name="errorText">Text description of the error.</param>
        public static void AddError(this ITokenBuilderContext context, Token token, string errorText)
        {
            Guard.NotNull(context, "context");
            Guard.NotNull(token, "token");
            AddError(context, token.LineIndex, token.ColumnIndex, errorText);
        }

        /// <summary>
        /// Adds error related to the <paramref name="tokenAttribute"/> described by the <paramref name="errorText"/>.
        /// </summary>
        /// <param name="context">Builder context.</param>
        /// <param name="tokenAttribute">The attribute which caused the error.</param>
        /// <param name="errorText">Text description of the error.</param>
        public static void AddError(this ITokenBuilderContext context, TokenAttribute tokenAttribute, string errorText)
        {
            Guard.NotNull(context, "context");
            Guard.NotNull(tokenAttribute, "token");
            AddError(context, tokenAttribute.OwnerToken.LineIndex, tokenAttribute.OwnerToken.ColumnIndex, errorText);
        }

        /// <summary>
        /// Parses content using <see cref="IParserService"/> and creates AST.
        /// </summary>
        /// <param name="context">Builder context.</param>
        /// <param name="content">Template content.</param>
        /// <returns>Parsed code object; <c>null</c> otherwise.</returns>
        public static ICodeObject TryProcessContent(this ITokenBuilderContext context, ISourceContent content)
        {
            return context.ParserContext.ParserService.ProcessContent(
                context.ParserContext.Name,
                content,
                context.ParserContext
            );
        }

        /// <summary>
        /// Parses value using <see cref="IParserService"/> and creates AST.
        /// </summary>
        /// <param name="context">Builder context.</param>
        /// <param name="value">Part of template content.</param>
        /// <returns>Parsed code object; <c>null</c> otherwise.</returns>
        public static ICodeObject TryProcessValue(this ITokenBuilderContext context, ISourceContent value)
        {
            return context.ParserContext.ParserService.ProcessValue(
                context.ParserContext.Name,
                value,
                context.ParserContext
            );
        }

        /// <summary>
        /// Parses value using property builder.
        /// </summary>
        /// <param name="context">Builder context.</param>
        /// <param name="propertyInfo">Property to build.</param>
        /// <param name="value">Value.</param>
        /// <returns>Parsed property descriptors.</returns>
        public static IEnumerable<ICodeProperty> TryProcessProperty(this ITokenBuilderContext context, IPropertyInfo propertyInfo, ISourceContent value)
        {
            Guard.NotNull(context, "context");
            return context.Registry
                .WithPropertyBuilder()
                .TryParse(new PropertyBuilderContext(context.ParserContext.Name, context.ParserContext, context.ParserContext.ParserService, propertyInfo), value);
        }
    }
}
