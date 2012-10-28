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
        IEnumerable<object> GetComponents();

        ICollection<object> GetComponents(object owner, string propertyName);

        void SetRootComponent(object component, Action propertyBinder);

        void AddComponent(object owner, string propertyName, object component, Action propertyBinder);

        bool RemoveComponent(object owner, string propertyName, object component);

        void AttachObserver(IControl control, IObserver observer, Action propertyBinder);

        void Init(object control);

        void Render(object control, HtmlTextWriter writer);

        void Dispose(object component);
    }
}
