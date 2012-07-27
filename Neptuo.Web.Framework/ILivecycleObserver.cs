using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework
{
    public interface ILivecycleObserver
    {
        IEnumerable<object> GetControls();

        void Register(object parent, object control);

        void Register(object parent, object control, Action propertyBinder);

        void Init(object control);

        void Render(object control, HtmlTextWriter writer);

        void Dispose(object control);
    }
}
