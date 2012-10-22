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
        protected ILivecycleObserver livecycleObserver;
        protected IServiceProvider serviceProvider;

        public void Setup(IViewPage viewPage, ILivecycleObserver livecycleObserver, IServiceProvider serviceProvider, HttpRequest request, HttpResponse response)
        {
            this.viewPage = viewPage;
            this.livecycleObserver = livecycleObserver;
            this.serviceProvider = serviceProvider;
            this.request = request;
            this.response = response;
        }

        public void CreateControls()
        {
            livecycleObserver.Register(null, viewPage, null);

            CreateControlsOverride();
        }

        protected abstract void CreateControlsOverride();

        public void Init()
        {
            InitOverride();

            viewPage.OnInit();
        }

        protected abstract void InitOverride();

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
