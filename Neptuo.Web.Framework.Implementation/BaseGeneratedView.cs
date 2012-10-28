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
        protected HttpRequest request;
        protected HttpResponse response;
        protected IViewPage viewPage;
        protected IComponentManager componentManager;
        protected IServiceProvider serviceProvider;

        public void Setup(IViewPage viewPage, IComponentManager componentManager, IServiceProvider serviceProvider, HttpRequest request, HttpResponse response)
        {
            this.viewPage = viewPage;
            this.componentManager = componentManager;
            this.serviceProvider = serviceProvider;
            this.request = request;
            this.response = response;
        }

        public void CreateControls()
        {
            componentManager.SetRootComponent(viewPage, CreateViewPageControls);
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
