using DemoWebUI.Models;
using Neptuo.Web.Framework;
using Neptuo.Web.Framework.Compilation;
using Neptuo.Web.Framework.Compilation.CodeGenerators;
using Neptuo.Web.Framework.Compilation.CodeGenerators.Extensions;
using Neptuo.Web.Framework.Compilation.CodeGenerators.Extensions.CodeDom;
using Neptuo.Web.Framework.Compilation.CodeObjects;
using Neptuo.Web.Framework.Compilation.Parsers;
using Neptuo.Web.Framework.Configuration;
using Neptuo.Web.Framework.Controls;
using Neptuo.Web.Framework.Observers;
using Neptuo.Web.Framework.Unity;
using Neptuo.Web.Framework.Utils;
using SignalR.Hubs;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace DemoWebUI.Hubs
{
    public class LiveHub : Hub
    {
        private IRegistrator Registrator { get; set; }
        private ExtendedCodeDomViewService ViewService { get; set; }
        private IDependencyContainer DependencyContainer { get; set; }

        public LiveHub()
        {
            IDependencyContainer dependencyContainer = new UnityDependencyContainer();
            Registrator registrator = new Registrator();
            registrator.LoadSection();

            XmlContentParser.LiteralTypeDescriptor literal = XmlContentParser.LiteralTypeDescriptor.Create<LiteralControl>(c => c.Text);
            XmlContentParser.GenericContentTypeDescriptor genericContent = XmlContentParser.GenericContentTypeDescriptor.Create<GenericContentControl>(c => c.TagName);

            ExtendedCodeDomViewService viewService = new ExtendedCodeDomViewService();
            //viewService.DebugMode = true;
            viewService.BinDirectories.Add(HttpContext.Current.Server.MapPath("~/Bin"));
            viewService.TempDirectory = @"C:\Temp\NeptuoFramework";
            viewService.DebugMode = true;
            viewService.ParserService.ContentParsers.Add(new XmlContentParser(literal, genericContent));
            viewService.ParserService.ValueParsers.Add(new ExtensionValueParser());
            viewService.CodeDomGenerator.SetCodeObjectExtension(typeof(ExtensionCodeObject), new ExtensionCodeObjectExtension());

            dependencyContainer
                .RegisterInstance<IDependencyProvider>(dependencyContainer)
                .RegisterInstance<IRegistrator>(registrator)
                .RegisterInstance<IViewService>(viewService)
                //.RegisterInstance<IVirtualPathProvider>()
                .RegisterType<IComponentManager, ComponentManager>();

            Registrator = registrator;
            ViewService = viewService;
            DependencyContainer = dependencyContainer;
        }

        public bool Compile(string viewContent)
        {
            ViewService.OnSourceCodeGenerated += (sender, e) =>
            {
                Caller.SourceCodeOutput(e.SourceCode);
            };

            try
            {
                IGeneratedView view = ViewService.ProcessContent(viewContent, new DefaultViewServiceContext(DependencyContainer));
                IComponentManager componentManager = DependencyContainer.Resolve<IComponentManager>();

                StringWriter output = new StringWriter();

                view.Setup(new BaseViewPage(componentManager), componentManager, DependencyContainer);
                view.CreateControls();
                view.Init();
                view.Render(new HtmlTextWriter(output));
                view.Dispose();

                Caller.Output(output.ToString());
            }
            catch (CodeDomViewServiceException e)
            {
                Caller.ShowErrors(e.Errors);
            }

            return true;
        }
    }
}