using Neptuo.Templates.Controls;
using Neptuo.Templates.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Observers
{
    /// <summary>
    /// EventArgs used in <see cref="IObserver"/>.
    /// </summary>
    public class ObserverEventArgs : EventArgs
    {
        /// <summary>
        /// Observer target control.
        /// </summary>
        public IControl Target { get; private set; }

        /// <summary>
        /// Current component manager.
        /// </summary>
        public IComponentManager ComponentManager { get; private set; }

        /// <summary>
        /// Flag to indicate whether <see cref="Target"/> can be processed.
        /// </summary>
        public bool Cancel { get; set; }

        public ObserverEventArgs(IControl target, IComponentManager componentManager)
        {
            Guard.NotNull(target, "target");
            Guard.NotNull(componentManager, "componentManager");
            Target = target;
            ComponentManager = componentManager;
        }
    }
}
