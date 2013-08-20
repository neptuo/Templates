using Neptuo.Templates.Controls;
using Neptuo.Templates.Observers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates
{
    /// <summary>
    /// Component manager.
    /// </summary>
    public interface IComponentManager
    {
        void AddComponent<T>(T component, Action<T> propertyBinder);

        void AttachObserver<T>(IControl control, T observer, Action<T> propertyBinder)
            where T : IObserver;

        void AttachInitComplete(IControl control, OnInitComplete handler);

        void Init(object control);

        void Render(object control, HtmlTextWriter writer);

        void Dispose(object component);
    }

    public delegate void OnInitComplete(ControlInitCompleteEventArgs e);

    public class ControlInitCompleteEventArgs : EventArgs
    {
        public IControl Target { get; private set; }

        public ControlInitCompleteEventArgs(IControl target)
        {
            Target = target;
        }
    }
}
