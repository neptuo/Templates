using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.ViewActivators
{
    /// <summary>
    /// Context for view activation.
    /// </summary>
    public interface IViewActivatorServiceContext
    {
        /// <summary>
        /// Current dependency provider.
        /// </summary>
        IDependencyProvider DependencyProvider { get; }

        /// <summary>
        /// Collection of errors.
        /// </summary>
        ICollection<IErrorInfo> Errors { get; }
    }
}
