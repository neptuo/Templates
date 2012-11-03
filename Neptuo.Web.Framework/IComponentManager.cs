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

        void Init(object control);

        void Render(object control, HtmlTextWriter writer);

        void Dispose(object component);
    }
}
