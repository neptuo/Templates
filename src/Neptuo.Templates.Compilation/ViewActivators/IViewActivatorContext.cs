using Neptuo.Activators;
using Neptuo.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.ViewActivators
{
    /// <summary>
    /// Context for view activation.
    /// </summary>
    public interface IViewActivatorContext
    {
        /// <summary>
        /// Current view activator service.
        /// </summary>
        IViewActivatorService ActivatorService { get; }

        /// <summary>
        /// Current dependency provider.
        /// </summary>
        IDependencyProvider DependencyProvider { get; }

        /// <summary>
        /// Collections of errors.
        /// </summary>
        ICollection<IErrorInfo> Errors { get; }
    }
}
