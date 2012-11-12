using DemoWebUI.Models;
using Neptuo.Web.Framework;
using Neptuo.Web.Framework.Compilation.CodeGenerators;
using Neptuo.Web.Framework.Compilation.CodeGenerators.Extensions;
using Neptuo.Web.Framework.Compilation.CodeObjects;
using Neptuo.Web.Framework.Compilation.Parsers;
using Neptuo.Web.Framework.Controls;
using Neptuo.Web.Framework.Observers;
using Neptuo.Web.Framework.Utils;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace DemoWebUI.Controllers
{
    public class LiveController : Controller
    {
        IComponentManager componentManager;
        IRegistrator registrator;
        IServiceProvider serviceProvider;

        public LiveController()
        {
            componentManager = new ComponentManager();
            registrator = new Registrator();
            serviceProvider = new ServiceProvider(registrator, componentManager);

            registrator.RegisterNamespace("h", "Neptuo.Web.Framework.Controls");
            registrator.RegisterNamespace(null, "Neptuo.Web.Framework.Controls");
            registrator.RegisterNamespace(null, "Neptuo.Web.Framework.Extensions");
            registrator.RegisterObserver("ui", "visible", typeof(VisibleObserver));
            registrator.RegisterObserver("html", "*", typeof(HtmlAttributeObserver));
            registrator.RegisterObserver("val", "max-length", typeof(ValidationObserver));
            registrator.RegisterObserver("val", "min-length", typeof(ValidationObserver));
            registrator.RegisterObserver("val", "regex", typeof(ValidationObserver));
            registrator.RegisterObserver("val", "message", typeof(ValidationObserver));
        }

        public ActionResult Index()
        {
            return View(new LiveModel
            {
                ViewContent = GetDefaultView()
            });
        }

        [HttpPost]
        public string Index(LiveModel model)
        {
            string sourceCode = GenerateCode(model.ViewContent);
            if (sourceCode == null)
                return "Error generating source code!";

            Assembly views = CompileCode(sourceCode);
            if (views == null)
                return "Error compiling assembly!";

            string result = RunCode(views);
            if (result == null)
                return "Error runing code!";

            return result;
        }

        private string GenerateCode(string viewContent)
        {
            XmlContentParser.LiteralTypeDescriptor literal = XmlContentParser.LiteralTypeDescriptor.Create<LiteralControl>(c => c.Text);
            XmlContentParser.GenericContentTypeDescriptor genericContent = XmlContentParser.GenericContentTypeDescriptor.Create<GenericContentControl>(c => c.TagName);

            IParserService parserService = new DefaultParserService();
            parserService.ContentParsers.Add(new XmlContentParser(literal, genericContent));
            parserService.ValueParsers.Add(new ExtensionValueParser());

            CodeDomGenerator generator = new CodeDomGenerator();
            generator.SetCodeObjectExtension(typeof(ExtensionCodeObject), new ExtensionCodeDomCodeObjectExtension());

            ICodeGeneratorService generatorService = new DefaultCodeGeneratorService();
            generatorService.AddGenerator("CSharp", generator);


            IPropertyDescriptor contentProperty = new ListAddPropertyDescriptor(typeof(BaseViewPage).GetProperty(TypeHelper.PropertyName<BaseViewPage>(v => v.Content)));
            bool parserResult = parserService.ProcessContent(viewContent, new DefaultParserServiceContext(serviceProvider, contentProperty));
            if (!parserResult)
            {
                Console.WriteLine("Parsing failed!");
                return null;
            }

            TextWriter writer = new StringWriter();
            bool generatorResult = generatorService.GeneratedCode("CSharp", contentProperty, new DefaultCodeGeneratorContext(writer));
            if (!generatorResult)
                return null;
            else
                return writer.ToString();
        }

        private Assembly CompileCode(string sourceCode)
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CompilerParameters cp = new CompilerParameters();
            cp.ReferencedAssemblies.Add(Server.MapPath("~/Bin/System.dll"));
            cp.ReferencedAssemblies.Add(Server.MapPath("~/Bin/System.Web.dll"));
            cp.ReferencedAssemblies.Add(Server.MapPath("~/Bin/Neptuo.Web.Framework.dll"));
            cp.ReferencedAssemblies.Add(Server.MapPath("~/Bin/Neptuo.Web.Framework.Implementation.dll"));
            cp.GenerateExecutable = false;
            cp.GenerateInMemory = true;
            cp.IncludeDebugInformation = true;

            CompilerResults cr = provider.CompileAssemblyFromSource(cp, sourceCode);
            if (cr.Errors.Count > 0)
            {
                foreach (CompilerError ce in cr.Errors)
                {
                    Console.WriteLine("  {0}", ce.ToString());
                    Console.WriteLine();
                }
                return null;
            }

            return cr.CompiledAssembly;
        }

        private string RunCode(Assembly views)
        {
            Type generatedView = views.GetType("Neptuo.Web.Framework.GeneratedView");

            StringWriter output = new StringWriter();

            IGeneratedView view = (IGeneratedView)Activator.CreateInstance(generatedView);
            view.Setup(new BaseViewPage(componentManager), componentManager, serviceProvider, null, null);
            view.CreateControls();
            view.Init();
            view.Render(new HtmlTextWriter(output));

            return output.ToString();
        }

        private string GetDefaultView()
        {
            return @"<html>
    <head>
        <title>Hello, World!</title>
    </head>
    <body ui:visible=""true"" class=""ui-body"">
        <h:panel html:id=""main-panel"" html:style=""float: left;"">
            <div>
                <h:textbox name=""surname"" text=""{Binding John}"" val:min-length=""1"" val:max-length=""10"" val:message=""Provide a valid value!"" />
            </div>
            <div>
                <h:textbox name=""surname"" />
            </div>
            
            <h:anchor url=""~/Admin"">
                <parameters>
                    <h:parameter name=""ID"" value=""5"" />
                    <h:parameter name=""Detail"" value=""True"" />
                </parameters>
                Hello World!
            </h:anchor>
        </h:panel>
    </body>
</html>";
        }
    }

    class ServiceProvider : IServiceProvider
    {
        private IRegistrator registrator;
        private IComponentManager componentManager;

        public ServiceProvider(IRegistrator registrator, IComponentManager componentManager)
        {
            this.registrator = registrator;
            this.componentManager = componentManager;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(IRegistrator))
                return registrator;

            if (serviceType == typeof(IComponentManager))
                return componentManager;

            return null;
        }
    }
}
