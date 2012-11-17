using Neptuo.Web.Framework.Controls;
using Neptuo.Web.Framework.Observers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework
{
    /// <summary>
    /// Component manager.
    /// </summary>
    public interface IComponentManager
    {
        void AddComponent(object component, Action propertyBinder);

        void AttachObserver(IControl control, IObserver observer, Action propertyBinder);

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
