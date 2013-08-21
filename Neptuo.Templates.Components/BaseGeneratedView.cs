using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Neptuo.Templates
{
    /// <summary>
    /// Base class for generated views.
    /// </summary>
    public abstract class BaseGeneratedView
    {
        protected IViewPage viewPage;
        protected IComponentManager componentManager;
        protected IDependencyProvider dependencyProvider;

        public ICollection<object> Content
        {
            get { return viewPage.Content; }
        }

        public void Setup(IViewPage viewPage, IComponentManager componentManager, IDependencyProvider dependencyProvider)
        {
            this.viewPage = viewPage;
            this.componentManager = componentManager;
            this.dependencyProvider = dependencyProvider;
        }

        public void CreateControls()
        {
            componentManager.AddComponent(viewPage, CreateViewPageControls);
        }

        protected abstract void CreateViewPageControls(IViewPage viewPage);

        public void Init()
        {
            componentManager.Init(viewPage);
        }

        public void Render(IHtmlWriter writer)
        {
            viewPage.Render(writer);
        }

        public void Dispose()
        {
            viewPage.Dispose();
        }
    }
}
