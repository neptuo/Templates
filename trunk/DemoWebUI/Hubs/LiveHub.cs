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
        private IViewService ViewService { get; set; }

        public LiveHub()
        {
            IDependencyContainer dependencyContainer = new UnityDependencyContainer();
            Registrator registrator = new Registrator();
            registrator.LoadSection();

            XmlContentParser.LiteralTypeDescriptor literal = XmlContentParser.LiteralTypeDescriptor.Create<LiteralControl>(c => c.Text);
            XmlContentParser.GenericContentTypeDescriptor genericContent = XmlContentParser.GenericContentTypeDescriptor.Create<GenericContentControl>(c => c.TagName);

            CodeDomViewService viewService = new CodeDomViewService();
            //viewService.DebugMode = true;
            viewService.BinDirectories = HttpContext.Current.Server.MapPath("~/Bin");
            viewService.TempDirectory = @"C:\Temp\NeptuoFramework";
            viewService.DebugMode
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
        }

        public bool Compile(string viewContent)
        {
            //TODO: Save as file in temp
            //TODO: Compile using ViewService


            string sourceCode = GenerateCode(viewContent);
            if (sourceCode == null)
            {
                Caller.Alert("Error generating source code!");
                return false;
            }
            Caller.SourceCodeOutput(sourceCode);

            Assembly views = CompileCode(sourceCode);
            if (views == null)
            {
                Caller.Alert("Error compiling assembly!");
                return false;
            }

            string result = RunCode(views);
            if (result == null)
            {
                Caller.Alert("Error runing code!");
                return false;
            }
            Caller.Output(result);

            return true;
        }
    }
}