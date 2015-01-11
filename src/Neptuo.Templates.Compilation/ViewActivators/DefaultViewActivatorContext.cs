using Neptuo.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.ViewActivators
{
    /// <summary>
    /// Implementation of <see cref="IViewActivatorContext"/>.
    /// </summary>
    public class DefaultViewActivatorContext : IViewActivatorContext
    {
        public IViewActivatorService ActivatorService { get; private set; }
        public IDependencyProvider DependencyProvider { get; private set; }
        public ICollection<IErrorInfo> Errors { get; private set; }

        public DefaultViewActivatorContext(IViewActivatorService activatorService, IViewActivatorServiceContext context)
        {
            Guard.NotNull(activatorService, "activatorService");
            ActivatorService = activatorService;
            DependencyProvider = context.DependencyProvider;
            Errors = context.Errors;
        }
    }
}
