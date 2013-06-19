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

        public PlaceHolderControl(ContentStorage storage)
        {
            this.storage = storage;
        }

        public void OnInit()
        {
            ContentStorageItem storageItem = storage.Peek();

            if (String.IsNullOrEmpty(Name))
                Name = String.Empty;

            if (storageItem.ContainsKey(Name))
            {
                foreach (object content in storageItem.Get(Name).Content)
                    ComponentManager.Init(content);
            }
        }

        public void Render(HtmlTextWriter writer)
        {
            ContentStorageItem storageItem = storage.Peek();
            if (storageItem.ContainsKey(Name))
            {
                foreach (object content in storageItem.Get(Name).Content)
                    ComponentManager.Render(content, writer);
            }
        }
    }
}
