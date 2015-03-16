using Test.Templates.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neptuo;

namespace Test.Templates
{
    /// <summary>
    /// EventArgs used in <see cref="IControlObserver"/>.
    /// </summary>
    public class DefaultControlObserverContext : EventArgs, IControlObserverContext
    {
        public IControl Target { get; private set; }
        public IComponentManager ComponentManager { get; private set; }
        public bool Cancel { get; set; }

        public DefaultControlObserverContext(IControl target, IComponentManager componentManager)
        {
            Ensure.NotNull(target, "target");
            Ensure.NotNull(componentManager, "componentManager");
            Target = target;
            ComponentManager = componentManager;
        }
    }
}
