using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Neptuo.Web.Framework
{
    /// <summary>
    /// Base class for generated views.
    /// </summary>
    public abstract class BaseGeneratedView : IGeneratedView
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

        protected abstract void CreateViewPageControls();

        public void Init()
        {
            componentManager.Init(viewPage);
        }

        public void Render(HtmlTextWriter writer)
        {
            viewPage.Render(writer);
        }

        public void Dispose()
        {
            viewPage.Dispose();
        }
    }
}
