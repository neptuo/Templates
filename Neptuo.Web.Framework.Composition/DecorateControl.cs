using Neptuo.Web.Framework.Compilation;
using Neptuo.Web.Framework.Composition.Data;
using Neptuo.Web.Framework.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Composition
{
    [DefaultProperty("Content")]
    public class DecorateControl : IControl
    {
        private IComponentManager componentManager;

        private ContentStorage storage;
        private ContentStorageItem storageItem;

        public ITemplate Template { get; set; }
        public ICollection<ContentControl> Content { get; set; }

        public DecorateControl(ContentStorage storage, IComponentManager componentManager)
        {
            this.storage = storage;
            this.componentManager = componentManager;
        }

        public void OnInit()
        {
            storageItem = storage.CreateItem();
            storage.Push(storageItem);
            if (Content != null)
            {
                foreach (ContentControl content in Content)
                {
                    componentManager.Init(content);
                    if (String.IsNullOrEmpty(content.Name))
                        content.Name = String.Empty;

                    storageItem.Add(content.Name, content);
                }
            }

            if (Template != null)
            {
                componentManager.Init(Template);
                Template.Init(componentManager);
            }
            storage.Pop();
        }

        public void Render(HtmlTextWriter writer)
        {
            storage.Push(storageItem);

            if (Template != null)
                Template.Render(componentManager, writer);

            storage.Pop();
        }
    }
}
