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
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            UnityServiceProvider serviceProvider = new UnityServiceProvider();
            Registrator registrator = new Registrator();
            registrator.LoadSection();

            XmlContentParser.LiteralTypeDescriptor literal = XmlContentParser.LiteralTypeDescriptor.Create<LiteralControl>(c => c.Text);
            XmlContentParser.GenericContentTypeDescriptor genericContent = XmlContentParser.GenericContentTypeDescriptor.Create<GenericContentControl>(c => c.TagName);

            CodeDomGenerator generator = new CodeDomGenerator();
            generator.SetCodeObjectExtension(typeof(ExtensionCodeObject), new ExtensionCodeDomCodeObjectExtension());

            CodeDomViewService viewService = new CodeDomViewService(serviceProvider);
            //viewService.DebugMode = true;
            viewService.BinDirectory = Server.MapPath("~/Bin");
            viewService.TempDirectory = @"C:\Temp\NeptuoFramework";
            viewService.ParserService.ContentParsers.Add(new XmlContentParser(literal, genericContent));
            viewService.ParserService.ValueParsers.Add(new ExtensionValueParser());
            viewService.CodeGeneratorService.AddGenerator("CSharp", generator);

            serviceProvider.Container
                .RegisterInstance<IServiceProvider>(serviceProvider)
                .RegisterInstance<IRegistrator>(registrator)
                .RegisterInstance<IViewService>(viewService)
                .RegisterType<IComponentManager, ComponentManager>();

            ViewEngines.Engines.Insert(0, new ViewEngine(serviceProvider));
        }
    }
}