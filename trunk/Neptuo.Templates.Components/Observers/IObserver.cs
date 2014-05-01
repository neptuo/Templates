using Neptuo.Templates.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Observers
{
    /// <summary>
    /// Interface that enables usage of type as observer.
    /// </summary>
    public interface IObserver
    {
        /// <summary>
        /// Observes target init phase.
        /// </summary>
        /// <param name="e">Context that describes observer usage.</param>
        void OnInit(ObserverEventArgs e);

        /// <summary>
        /// Observes target render phase.
        /// </summary>
        /// <param name="e">Context that describes observer usage.</param>
        /// <param name="writer">Output writer.</param>
        void Render(ObserverEventArgs e, IHtmlWriter writer);
    }
}
