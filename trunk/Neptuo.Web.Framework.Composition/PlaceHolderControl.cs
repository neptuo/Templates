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
        private IComponentManager componentManager;

        public string Name { get; set; }

        public PlaceHolderControl(ContentStorage storage, IComponentManager componentManager)
        {
            this.storage = storage;
            this.componentManager = componentManager;
        }

        public void OnInit()
        {
            ContentStorageItem storageItem = storage.Peek();

            if (String.IsNullOrEmpty(Name))
                Name = String.Empty;

            if (storageItem.ContainsKey(Name))
            {
                foreach (object content in storageItem.Get(Name).Content)
                    componentManager.Init(content);
            }
        }

        public void Render(HtmlTextWriter writer)
        {
            ContentStorageItem storageItem = storage.Peek();
            if (storageItem.ContainsKey(Name))
            {
                foreach (object content in storageItem.Get(Name).Content)
                    componentManager.Render(content, writer);
            }
        }
    }
}
