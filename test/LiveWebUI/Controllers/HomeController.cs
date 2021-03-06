﻿using LiveWebUI.Models;
using Neptuo;
using Neptuo.Templates;
using Neptuo.Templates.Compilation;
using Neptuo.Templates.Compilation.Parsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewPage = Neptuo.Templates.ViewPage;

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

            GeneratedView view = (GeneratedView)TemplateHelper.ViewService.ProcessContent("CSharp", new DefaultSourceContent(viewContent), new DefaultViewServiceContext(TemplateHelper.Container));
            view.Setup(new ViewPage(TemplateHelper.Container.Resolve<IComponentManager>()), TemplateHelper.Container.Resolve<IComponentManager>(), TemplateHelper.Container);
            view.CreateControls();
            view.Init();
            view.Render(new HtmlTextWriter(output));
            view.Dispose();
            return output.ToString();
        }

        public ActionResult RunClient(string identifier)
        {
            string viewContent = TemplateHelper.ViewRepository.GetContent(identifier);
            if (String.IsNullOrEmpty(viewContent))
                return Content("Missing view content!");

            INaming naming = TemplateHelper.ViewService.NamingService.FromContent(viewContent);
            string javascript = TemplateHelper.ViewService.GenerateJavascript(new DefaultSourceContent(viewContent), new DefaultViewServiceContext(TemplateHelper.Container), naming);
            
            return View(new JavascriptModel(javascript, String.Join(".", naming.ClassNamespace, naming.ClassName)));
        }
    }
}
