using Neptuo.ComponentModel;
using Neptuo.Templates.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates
{
    /// <summary>
    /// Base implmenetation of <see cref="IViewPage"/>.
    /// </summary>
    public class ViewPage : DisposableBase, IViewPage
    {
        public IComponentManager ComponentManager { get; set; }

        /// <summary>
        /// Root content of view page.
        /// </summary>
        public ICollection<object> Content { get; set; }

        public ViewPage()
        {
            Content = new List<object>();
        }

        /// <summary>
        /// Initializes <see cref="Content"/>.
        /// </summary>
        /// <param name="componentManager">Current component manager.</param>
        public void OnInit(IComponentManager componentManager)
        {
            Guard.NotNull(componentManager, "componentManager");
            ComponentManager = componentManager;

            foreach (object item in Content)
                ComponentManager.Init(item);
        }

        /// <summary>
        /// Renders <see cref="Content"/> to <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">Output writer.</param>
        public void Render(IHtmlWriter writer)
        {
            Guard.NotNull(writer, "writer");

            foreach (object item in Content)
                ComponentManager.Render(item, writer);
        }

        /// <summary>
        /// Disposes this view.
        /// </summary>
        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();

            foreach (object item in Content)
                ComponentManager.Dispose(item);
        }
    }
}
