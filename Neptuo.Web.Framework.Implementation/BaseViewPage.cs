using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework
{
    public class BaseViewPage : IViewPage
    {
        public IComponentManager ComponentManager { get; set; }

        public ICollection<object> Content { get; set; }

        public BaseViewPage(IComponentManager componentManager)
        {
            ComponentManager = componentManager;
        }

        public void OnInit()
        {
            foreach (object item in Content)
                ComponentManager.Init(item);
        }

        public void Render(HtmlTextWriter writer)
        {
            foreach (object item in Content)
                ComponentManager.Render(item, writer);
        }

        public void Dispose()
        {
            foreach (object item in Content)
                ComponentManager.Dispose(item);
        }
    }
}
