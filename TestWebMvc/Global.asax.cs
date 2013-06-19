using Microsoft.Practices.Unity;
using Neptuo.Web.Framework.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Neptuo.Web.Framework.Mvc;
using Neptuo.Web.Framework.Unity;
using Neptuo.Web.Framework;
using Neptuo.Web.Framework.Compilation.Parsers;
using Neptuo.Web.Framework.Controls;
using Neptuo.Web.Framework.Compilation.CodeGenerators.Extensions;
using Neptuo.Web.Framework.Compilation.CodeGenerators;
using Neptuo.Web.Framework.Compilation.CodeObjects;
using Neptuo.Web.Framework.Compilation;
using Neptuo.Web.Framework.Compilation.CodeGenerators.Extensions.CodeDom;
using Neptuo.Web.Framework.Composition.Data;

namespace TestWebMvc
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            IDependencyContainer dependencyContainer = new UnityDependencyContainer();
            Registrator registrator = new Registrator();
            registrator.LoadSection();



            XmlContentParser.LiteralTypeDescriptor literal = XmlContentParser.LiteralTypeDescriptor.Create<LiteralControl>(c => c.Text);
            XmlContentParser.GenericContentTypeDescriptor genericContent = XmlContentParser.GenericContentTypeDescriptor.Create<GenericContentControl>(c => c.TagName);

            CodeDomViewService viewService = new CodeDomViewService();
            viewService.LoadSection();
            viewService.ParserService.ContentParsers.Add(new XmlContentParser(literal, genericContent));
            viewService.ParserService.ValueParsers.Add(new ExtensionValueParser());
            viewService.CodeDomGenerator.SetCodeObjectExtension(typeof(ExtensionCodeObject), new ExtensionCodeObjectExtension());


            ViewEngine viewEngine = new ViewEngine(dependencyContainer);

            dependencyContainer
                .RegisterInstance<IDependencyProvider>(dependencyContainer)
                .RegisterInstance<IRegistrator>(registrator)
                .RegisterInstance<IViewService>(viewService)
                .RegisterInstance<IVirtualPathProvider>(viewEngine)
                .RegisterInstance<ContentStorage>(new ContentStorage())
                .RegisterType<IFileProvider, FileProvider>()
                .RegisterType<IComponentManager, ComponentManager>()
                .RegisterType<IRequestHelper, HttpContextRequestHelper>();

            ViewEngines.Engines.Insert(0, viewEngine);
        }
    }
}