using Neptuo.Templates.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Runtime
{
    /// <summary>
    /// EventArgs used in registeration of init complete event.
    /// </summary>
    public class ControlInitCompleteEventArgs : EventArgs
    {
        /// <summary>
        /// Target control.
        /// </summary>
        public IControl Target { get; private set; }

        public ControlInitCompleteEventArgs(IControl target)
        {
            Guard.NotNull(target, "target");
            Target = target;
        }
    }
}
