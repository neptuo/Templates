using Neptuo.Web.Framework.Composition.Data;
using Neptuo.Web.Framework.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Composition
{
    public class PlaceHolderControl : IControl
    {
        private ContentStorage storage;

        public IComponentManager ComponentManager { get; set; }
        public IDependencyProvider DependencyProvider { get; set; }

        public string Name { get; set; }

        public void OnInit()
        {
            storage = DependencyProvider.Resolve<ContentStorage>();
            if (storage.ContainsKey(Name))
            {
                foreach (object content in storage[Name].Content)
                    ComponentManager.Init(content);
            }
        }

        public void Render(HtmlTextWriter writer)
        {
            if (storage.ContainsKey(Name))
            {
                foreach (object content in storage[Name].Content)
                    ComponentManager.Render(content, writer);
            }
        }
    }
}
