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
        /// Root property descriptor.
        /// </summary>
        IPropertyDescriptor PropertyDescriptor { get; }

        /// <summary>
        /// List of error messages.
        /// </summary>
        ICollection<IErrorInfo> Errors { get; }
    }
}
