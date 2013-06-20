using Neptuo.Web.Framework.Compilation;
using Neptuo.Web.Framework.Compilation.CodeGenerators.Extensions.CodeDom;
using Neptuo.Web.Framework.Compilation.CodeObjects;
using Neptuo.Web.Framework.Compilation.Parsers;
using Neptuo.Web.Framework.Controls;
using Neptuo.Web.Framework.Mvc.Data;
using Neptuo.Web.Framework.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

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
            IViewDataContainer viewDataContainer = new TempViewDataContainer(viewContext);
            IComponentManager componentManager = dependencyProvider.Resolve<IComponentManager>();
            IDependencyContainer container = dependencyProvider.CreateChildContainer();
            container.RegisterInstance(viewContext);
            container.RegisterInstance<IViewDataContainer>(viewDataContainer);
            container.RegisterInstance(new DataStorage(viewContext.ViewData.Model));
            container.RegisterInstance(new HtmlHelper(viewContext, viewDataContainer));
            container.RegisterInstance(new UrlHelper(viewContext.RequestContext, RouteTable.Routes));

            IGeneratedView view = viewService.Process(ViewName, new DefaultViewServiceContext(container));
            view.Setup(new BaseViewPage(componentManager), componentManager, container);
            view.CreateControls();
            view.Init();
            view.Render(new HtmlTextWriter(writer));
            view.Dispose();
        }
    }

    internal class TempViewDataContainer : IViewDataContainer
    {
        private ViewContext viewContext;

        public ViewDataDictionary ViewData
        {
            get { return viewContext.ViewData; }
            set { viewContext.ViewData = value; }
        }

        public TempViewDataContainer(ViewContext viewContext)
        {
            this.viewContext = viewContext;
        }
    }

}
