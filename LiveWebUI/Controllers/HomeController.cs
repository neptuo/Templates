using LiveWebUI.Hubs;
using Neptuo.Templates;
using Neptuo.Templates.Compilation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiveWebUI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string identifier)
        {
            return View((object)(new MvcHtmlString(String.IsNullOrEmpty(identifier) ? "null" : String.Format("'{0}'", identifier))));
        }

        public string RunServer(string identifier)
        {
            string viewContent = TemplateHelper.ViewRepository.GetContent(identifier);
            if (String.IsNullOrEmpty(viewContent))
                return "Missing view content!";

            StringWriter output = new StringWriter();

            BaseGeneratedView view = (BaseGeneratedView)TemplateHelper.ViewService.ProcessContent(viewContent, new ViewServiceContext(TemplateHelper.Container));
            view.Setup(new BaseViewPage(TemplateHelper.Container.Resolve<IComponentManager>()), TemplateHelper.Container.Resolve<IComponentManager>(), TemplateHelper.Container);
            view.CreateControls();
            view.Init();
            view.Render(new HtmlTextWriter(output));
            view.Dispose();
            return output.ToString();
        }

    }
}
