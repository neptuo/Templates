using Neptuo.Web.Framework.Composition;
using Neptuo.Web.Framework.Controls;
using Neptuo.Web.Framework.Mvc.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Mvc.Controls
{
    [Html("table")]
    public class ListViewControl : BaseContentControl
    {
        private IEnumerable items;
        private IComponentManager componentManager;
        private DataStorage storage;

        public ITemplate HeadTemplate { get; set; }
        public object ItemsSource { get; set; }
        public ITemplate ItemTemplate { get; set; }

        public ListViewControl(IComponentManager componentManager, DataStorage storage)
        {
            this.componentManager = componentManager;
            this.storage = storage;
        }

        public override void OnInit()
        {
            base.OnInit();

            if (ItemsSource is IEnumerable)
                items = (IEnumerable)ItemsSource;
        }

        protected override void RenderBody(HtmlTextWriter writer)
        {
            if (items == null)
                return;

            if (HeadTemplate != null)
            {
                HeadTemplate.Init(componentManager);
                HeadTemplate.Render(componentManager, writer);
            }

            foreach (var item in items)
            {
                storage.Push(item);
                ItemTemplate.Init(componentManager);
                ItemTemplate.Render(componentManager, writer);
                storage.Pop();
            }
        }
    }
}
