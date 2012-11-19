using Neptuo.Web.Framework.Compilation;
using Neptuo.Web.Framework.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Composition
{
    [DefaultProperty("Name")]
    public class IncludeControl : IControl, IDisposable
    {
        private IGeneratedView view;

        public IComponentManager ComponentManager { get; set; }
        public IDependencyProvider DependencyProvider { get; set; }

        public string Path { get; set; }

        public void OnInit()
        {
            IViewService viewService = DependencyProvider.Resolve<IViewService>();
            view = viewService.Process(Path, new DefaultViewServiceContext(DependencyProvider));
            if (view != null)
            {
                view.Setup(new BaseViewPage(ComponentManager), ComponentManager, DependencyProvider);
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
