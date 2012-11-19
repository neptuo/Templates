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
        private IGeneratedView view;
        private IDependencyContainer childContainer;
        private ContentStorage storage = new ContentStorage();

        public IComponentManager ComponentManager { get; set; }
        public IDependencyProvider DependencyProvider { get; set; }

        public string Path { get; set; }
        public ICollection<ContentControl> Content { get; set; }

        public void OnInit()
        {
            if (Content != null)
            {
                foreach (ContentControl content in Content)
                {
                    if (String.IsNullOrEmpty(content.Name))
                        content.Name = String.Empty;

                    ComponentManager.Init(content);
                    storage.Add(content.Name, content);
                }
            }

            IViewService viewService = DependencyProvider.Resolve<IViewService>();
            view = viewService.Process(Path, new DefaultViewServiceContext(DependencyProvider));
            if (view != null)
            {
                childContainer = DependencyProvider.CreateChildContainer();
                childContainer.RegisterInstance<ContentStorage>(storage);

                view.Setup(new BaseViewPage(ComponentManager), ComponentManager, childContainer);
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
