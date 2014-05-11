using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Value parser context.
    /// </summary>
    public interface IValueParserContext
    {
        /// <summary>
        /// Current dependency provider.
        /// </summary>
        IDependencyProvider DependencyProvider { get; }

        /// <summary>
        /// Current parser service.
        /// </summary>
        IParserService ParserService { get; }

        /// <summary>
        /// Root property descriptor.
        /// </summary>
        IPropertyDescriptor PropertyDescriptor { get; }

        /// <summary>
        /// List of error messages.
        /// </summary>
        ICollection<IErrorInfo> Errors { get; }
    }
}
