using Neptuo.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Some common extensions for <see cref="IPropertyBuilderContext"/>.
    /// </summary>
    public static class _PropertyBuilderContextExtensions
    {
        /// <summary>
        /// Adds error to the parser context.
        /// </summary>
        /// <param name="context">Builder context.</param>
        /// <param name="errorInfo">Error description.</param>
        public static void AddError(this IPropertyBuilderContext context, IErrorInfo errorInfo)
        {
            Guard.NotNull(context, "context");
            Guard.NotNull(errorInfo, "errorInfo");
            context.Errors.Add(errorInfo);
        }

        /// <summary>
        /// Adds error to the parser context.
        /// </summary>
        /// <param name="context">Builder context.</param>
        /// <param name="errorText">Description of error.</param>
        public static void AddError(this IPropertyBuilderContext context, string errorText)
        {
            Guard.NotNull(context, "context");
            context.AddError(new ErrorInfo(1, 1, errorText));
        }
    }
}
