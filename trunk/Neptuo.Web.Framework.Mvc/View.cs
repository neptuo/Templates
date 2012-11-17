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
        private IServiceProvider serviceProvider;

        public string ViewName { get; protected set; }

        public string MasterName { get; protected set; }

        public bool UseCache { get; protected set; }

        public View(IViewService viewService, IServiceProvider serviceProvider, string viewName, string masterName = null, bool? useCache = null)
        {
            this.viewService = viewService;
            this.serviceProvider = serviceProvider;
            ViewName = viewName;
            MasterName = masterName;
            UseCache = useCache ?? true;
        }

        public void Render(ViewContext viewContext, TextWriter writer)
        {
            IComponentManager componentManager = serviceProvider.GetService<IComponentManager>();

            IGeneratedView view = viewService.Process(HttpContext.Current.Server.MapPath(ViewName));
            view.Setup(new BaseViewPage(componentManager), componentManager, serviceProvider);
            view.CreateControls();
            view.Init();
            view.Render(new HtmlTextWriter(writer));
        }
    }
}
