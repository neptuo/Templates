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
    public class TemplateControl : IControl, IDisposable
    {
        private IDependencyProvider provider;
        private IComponentManager componentManager;

        private IGeneratedView view;

        public string Template { get; set; }

        public string Path { get; set; }
        public ICollection<ContentControl> Content { get; set; }

        public TemplateControl(IDependencyProvider provider, IComponentManager componentManager)
        {
            this.provider = provider;
            this.componentManager = componentManager;
        }

        public void OnInit()
        {
            ContentStorage storage = new ContentStorage();
            if (Content != null)
            {
                foreach (ContentControl content in Content)
                {
                    if (String.IsNullOrEmpty(content.Name))
                        content.Name = String.Empty;

                    componentManager.Init(content);
                    storage.Add(content.Name, content);
                }
            }

            IViewService viewService = provider.Resolve<IViewService>();
            view = viewService.Process(Path, new DefaultViewServiceContext(provider));
            if (view != null)
            {
                IDependencyContainer childContainer = provider.CreateChildContainer();
                childContainer.RegisterInstance<ContentStorage>(storage);

                view.Setup(new BaseViewPage(componentManager), componentManager, childContainer);
                view.CreateControls();
                view.Init();
            }
        }

        public void Render(HtmlTextWriter writer)
        {
            if (view != null)
                view.Render(writer);
        }

        public void Dispose()
        {
            if (view != null)
                view.Dispose();
        }
    }
}
