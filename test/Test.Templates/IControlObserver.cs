﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.Templates
{
    /// <summary>
    /// Interface that enables usage of type as observer.
    /// </summary>
    public interface IControlObserver
    {
        /// <summary>
        /// Observes target init phase.
        /// </summary>
        /// <param name="context">Context that describes observer usage.</param>
        void OnInit(IControlObserverContext context);

        /// <summary>
        /// Observes target render phase.
        /// </summary>
        /// <param name="context">Context that describes observer usage.</param>
        /// <param name="writer">Output writer.</param>
        void Render(IControlObserverContext context, IHtmlWriter writer);
    }
}
