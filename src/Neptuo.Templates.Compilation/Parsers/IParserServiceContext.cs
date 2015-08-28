using Neptuo.Activators;
using Neptuo.Compilers.Errors;
using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Parser service context.
    /// </summary>
    public interface IParserServiceContext
    {
        /// <summary>
        /// Current dependency provider.
        /// </summary>
        IDependencyProvider DependencyProvider { get; }

        /// <summary>
        /// List of error messages.
        /// </summary>
        ICollection<IErrorInfo> Errors { get; }

        /// <summary>
        /// Factory method for creating <see cref="ITextContentParserContext"/>.
        /// </summary>
        /// <param name="name">Name of the pipeline to execute.</param>
        /// <param name="service">Current parser service.</param>
        /// <returns><see cref="ITextContentParserContext"/>.</returns>
        ITextContentParserContext CreateContentContext(string name, IParserService service);

        /// <summary>
        /// Factory method for creating <see cref="ITextValueParserContext"/>.
        /// </summary>
        /// <param name="name">Name of the pipeline to execute.</param>
        /// <param name="service">Current parser service.</param>
        /// <returns><see cref="ITextValueParserContext"/>.</returns>
        ITextValueParserContext CreateValueContext(string name, IParserService service);
    }
}
