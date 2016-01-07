using EnvDTE;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Neptuo;
using Neptuo.Windows.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Test.Templates.VisualStudio.UI
{
    /// <summary>
    /// Minitors current editor and provides properties from it.
    /// </summary>
    public class CurrentBufferPropertyProvider : DisposableBase
    {
        private readonly WindowEvents events;
        private readonly IVsTextManager viewManager;
        private readonly IVsEditorAdaptersFactoryService adapterService;

        private readonly List<Entry> storage = new List<Entry>();

        private ITextView currentTextView;

        public CurrentBufferPropertyProvider()
        {
            // Subscribe to document events.
            DTE dte = (DTE)ServiceProvider.GlobalProvider.GetService(typeof(DTE));
            events = dte.Events.WindowEvents;
            events.WindowActivated += OnWindowActivated;

            // Find view manager (for determining current view) and WPF view adapter.
            viewManager = (IVsTextManager)ServiceProvider.GlobalProvider.GetService(typeof(SVsTextManager));
            IComponentModel componentModel = (IComponentModel)ServiceProvider.GlobalProvider.GetService(typeof(SComponentModel));
            adapterService = componentModel.GetService<IVsEditorAdaptersFactoryService>();
        }

        public CurrentBufferPropertyProvider Add<T>(Action<T> handler)
        {
            Ensure.NotNull(handler, "handler");
            storage.Add(new Entry()
            {
                PropertyType = typeof(T),
                Handler = (o, view) => handler((T)o)
            });

            return this;
        }

        public CurrentBufferPropertyProvider Add<T>(Action<T, ITextView> handler)
        {
            Ensure.NotNull(handler, "handler");
            storage.Add(new Entry()
            {
                PropertyType = typeof(T),
                Handler = (o, view) => handler((T)o, view)
            });

            return this;
        }

        private void OnWindowActivated(Window gotFocus, Window lostFocus)
        {
            DispatcherHelper.Run(Dispatcher.CurrentDispatcher, () =>
            {
                ITextView textView;

                if (TryGetTextView(out textView))
                {
                    // If context is different.
                    if (currentTextView != textView)
                    {
                        // Store view for view operations.
                        currentTextView = textView;

                        foreach (Entry entry in storage)
                        {
                            object value;
                            if (textView.TextBuffer.Properties.TryGetProperty(entry.PropertyType, out value))
                                entry.Handler(value, textView);
                        }
                    }
                }
            }, 300);
        }

        /// <summary>
        /// Tries to get <paramref name="context"/> of currently selected document.
        /// </summary>
        /// <param name="context">The token context of current document.</param>
        /// <param name="textView">The current text view.</param>
        /// <returns><c>true</c> if both was found; <c>false</c> otherwise.</returns>
        private bool TryGetTextView(out ITextView textView)
        {
            IVsTextView view;
            if (viewManager.GetActiveView(1, null, out view) == 0)
            {
                textView = adapterService.GetWpfTextView(view);
                return true;
            }

            textView = null;
            return false;
        }

        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();
            events.WindowActivated -= OnWindowActivated;
        }

        private class Entry 
        {
            public Type PropertyType {get; set;}
            public Action<object, ITextView> Handler { get; set; }
        }
    }
}
