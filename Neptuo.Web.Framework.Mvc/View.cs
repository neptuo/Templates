using Neptuo.Web.Framework.Compilation;
using Neptuo.Web.Framework.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Neptuo.Web.Framework.Mvc
{
    /// <summary>
    /// Html view.
    /// </summary>
    public class View : IView
    {
        private IViewService viewService;
        private IDependencyProvider dependencyProvider;

        public string ViewName { get; protected set; }

        public string MasterName { get; protected set; }

        public bool UseCache { get; protected set; }

        public View(IViewService viewService, IDependencyProvider dependencyProvider, string viewName, string masterName = null, bool? useCache = null)
        {
            this.viewService = viewService;
            this.dependencyProvider = dependencyProvider;
            ViewName = viewName;
            MasterName = masterName;
            UseCache = useCache ?? true;
        }

        public void Render(ViewContext viewContext, TextWriter writer)
        {
            IComponentManager componentManager = dependencyProvider.Resolve<IComponentManager>();

            IGeneratedView view = viewService.Process(HttpContext.Current.Server.MapPath(ViewName), new DefaultViewServiceContext(dependencyProvider));
            view.Setup(new BaseViewPage(componentManager), componentManager, dependencyProvider);
            view.CreateControls();
            view.Init();
            view.Render(new HtmlTextWriter(writer));
            view.Dispose();
        }
    }
}
