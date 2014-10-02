using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation
{
    /// <summary>
    /// Implementaion of <see cref="IViewServiceContext"/>.
    /// </summary>
    public class DefaultViewServiceContext : IViewServiceContext
    {
        public IDependencyProvider DependencyProvider { get; private set; }
        public ICollection<IErrorInfo> Errors { get; private set; }

        public DefaultViewServiceContext(IDependencyProvider dependencyProvider, ICollection<IErrorInfo> errors = null)
        {
            Guard.NotNull(dependencyProvider, "dependencyProvider");
            DependencyProvider = dependencyProvider;
            Errors = errors ?? new List<IErrorInfo>();
        }
    }
}
