using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.ViewActivators
{
    /// <summary>
    /// Implementation of <see cref="IViewActivatorService"/>.
    /// </summary>
    public class DefaultViewActivatorService : IViewActivatorService
    {
        private readonly Dictionary<string, IViewActivator> activators = new Dictionary<string, IViewActivator>();

        public IViewActivatorService AddActivator(string name, IViewActivator activator)
        {
            Guard.NotNullOrEmpty(name, "name");
            Guard.NotNull(activator, "activator");
            activators[name] = activator;
            return this;
        }

        public object Activate(string name, string viewContent, IViewActivatorServiceContext context)
        {
            Guard.NotNullOrEmpty(name, "name");
            Guard.NotNullOrEmpty(viewContent, "viewContent");
            Guard.NotNull(context, "context");
            IViewActivator activator;
            if (activators.TryGetValue(name, out activator))
                return activator.Activate(viewContent, new DefaultViewActivatorContext(this, context));

            throw new ArgumentOutOfRangeException("name", String.Format("Requested unregistered view activator named '{0}'.", name));
        }
    }
}
