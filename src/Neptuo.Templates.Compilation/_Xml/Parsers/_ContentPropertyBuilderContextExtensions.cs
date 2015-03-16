using Neptuo.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Some common extensions for <see cref="IContentPropertyBuilderContext"/>.
    /// </summary>
    public static class _ContentPropertyBuilderContextExtensions
    {
        /// <summary>
        /// Adds error to the parser context.
        /// </summary>
        /// <param name="context">Builder context.</param>
        /// <param name="line">Source file line number.</param>
        /// <param name="column">Source file line column number.</param>
        /// <param name="errorText">Description of error.</param>
        public static void AddError(this IContentPropertyBuilderContext context, int line, int column, string errorText)
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
        public static void AddError(this IContentPropertyBuilderContext context, IXmlElement element, string errorText)
        {
            Ensure.NotNull(context, "context");
            Ensure.NotNull(element, "element");

            ISourceLineInfo lineInfo = element as ISourceLineInfo;
            if (lineInfo != null)
                context.AddError(lineInfo.LineIndex, lineInfo.ColumnIndex, errorText);
            else
                context.AddError(String.Format("<{0}> -> {1}", element.Name, errorText));
        }
    }
}
