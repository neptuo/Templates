using Neptuo.Activators;
using Neptuo.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation
{
    /// <summary>
    /// View service context.
    /// </summary>
    public interface IViewServiceContext
    {
        /// <summary>
        /// Current dependency provider.
        /// </summary>
        IDependencyProvider DependencyProvider { get; }

        /// <summary>
        /// Collection of errors that occured during processing view.
        /// </summary>
        ICollection<IErrorInfo> Errors { get; }
    }
}
