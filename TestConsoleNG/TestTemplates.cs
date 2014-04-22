using Neptuo;
using Neptuo.Linq.Expressions;
using Neptuo.Templates;
using Neptuo.Templates.Compilation;
using Neptuo.Templates.Compilation.CodeGenerators;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers;
using Neptuo.Templates.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestConsoleNG.Controls;
using TestConsoleNG.Data;
using TestConsoleNG.Extensions;
using TestConsoleNG.Observers;
using TestConsoleNG.SimpleContainer;
using TestConsoleNG.Unity;

namespace TestConsoleNG
{
    public static class TestTemplates
    {
        private static IDependencyContainer container;

        static TestTemplates()
        {
            container = new UnityDependencyContainer();
            container.RegisterInstance<IFileProvider>(new FileProvider(new CurrentDirectoryVirtualPathProvider()));
            container.RegisterInstance<DataStorage>(new DataStorage(new PersonModel("Jon", "Doe", new AddressModel("Dlouhá street", 23, "Prague", 10001))));
            container.RegisterInstance<IValueConverterService>(new ValueConverterService().SetConverter("NullToBool", new NullToBoolValueConverter()));
            container.RegisterType<IViewServiceContext, ViewServiceContext>();
        }

        public static void Test()
        {
            TypeBuilderRegistry registry = new TypeBuilderRegistry(
                new TypeBuilderRegistryConfiguration(container).AddComponentSuffix("presenter"),
                new LiteralControlBuilder<LiteralControl>(c => c.Text), 
                new GenericContentControlBuilder<GenericContentControl>(c => c.TagName)
            );
            registry.RegisterNamespace(new NamespaceDeclaration("h", "TestConsoleNG.Controls, TestConsoleNG.Components"));
            registry.RegisterNamespace(new NamespaceDeclaration(null, "TestConsoleNG.Extensions, TestConsoleNG.Components"));
            registry.RegisterObserverBuilder("data", "*", new DefaultTypeObserverBuilderFactory(typeof(DataContextObserver)));
            registry.RegisterObserverBuilder("ui", "Visible", new DefaultTypeObserverBuilderFactory(typeof(VisibleObserver)));
            registry.RegisterPropertyBuilder(typeof(string), new DefaultPropertyBuilderFactory<StringPropertyBuilder>());
            registry.RegisterPropertyBuilder(typeof(ITemplate), new DefaultPropertyBuilderFactory<TemplatePropertyBuilder>());

            CodeDomViewService viewService = new CodeDomViewService(true);
            viewService.ParserService.ContentParsers.Add(new XmlContentParser(registry));
            viewService.ParserService.DefaultValueParser = new PlainValueParser();
            viewService.ParserService.ValueParsers.Add(new MarkupExtensionValueParser(registry));
            viewService.NamingService = new HashNamingService(new FileProvider(new CurrentDirectoryVirtualPathProvider()));
            //viewService.DebugMode = true;
            viewService.TempDirectory = @"C:\Temp\NeptuoFramework";
            viewService.BinDirectories.Add(Environment.CurrentDirectory);

            viewService.JavascriptGenerator.TempDirectory = @"C:\Temp\NeptuoFramework";
            viewService.JavascriptGenerator.BinDirectories.Add(Environment.CurrentDirectory);

            IFieldNameProvider fieldNameProvider = new SequenceFieldNameProvider();
            viewService.CodeDomGenerator.RegisterStandartCodeGenerators(fieldNameProvider);
            viewService.CodeDomGenerator.SetCodeObjectGenerator(typeof(TemplateCodeObject), new CodeDomTemplateGenerator(fieldNameProvider));
            viewService.CodeDomGenerator.SetCodeObjectGenerator(typeof(MethodReferenceCodeObject), new CodeDomMethodReferenceGenerator());

            container.RegisterInstance<IViewService>(viewService);
            container.RegisterInstance<IComponentManager>(new ComponentManager());

            StringWriter output = new StringWriter();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //try
            //{

            IViewServiceContext context = new ViewServiceContext(container);





            
            //IPropertyInfo propertyInfo = new TypePropertyInfo(typeof(BaseGeneratedView).GetProperty(TypeHelper.PropertyName<BaseGeneratedView>(v => v.Content)));
            //IPropertyDescriptor contentProperty= new ListAddPropertyDescriptor(propertyInfo);
            //StringWriter javascriptOutput = new StringWriter();

            //viewService.ParserService.ProcessContent(File.ReadAllText("Index.html"), new DefaultParserServiceContext(container, contentProperty));
            //viewService.JavascriptGenerator.ProcessTree(contentProperty, new CodeDomGeneratorContext(new DefaultNaming("X", "X", "X", "X"), output, viewService.CodeGeneratorService, container));

            //Console.WriteLine(javascriptOutput);









            //BaseGeneratedView view = (BaseGeneratedView)viewService.ProcessContent("<h:panel class='checkin'><a href='google'>Hello, World!</a></h:panel>", context);
            BaseGeneratedView view = (BaseGeneratedView)viewService.Process("Index.html", context);
            DebugUtils.Run("Run", () =>
            {
                view.Setup(new BaseViewPage(container.Resolve<IComponentManager>()), container.Resolve<IComponentManager>(), container);
                view.CreateControls();
                view.Init();
                view.Render(new HtmlTextWriter(output));
                view.Dispose();
            });

            stopwatch.Stop();
            //}
            //catch (CodeDomViewServiceException e)
            //{
            //    output.WriteLine("Errors occured!");
            //    output.WriteLine();
            //    foreach (IErrorInfo error in e.Errors)
            //    {
            //        output.WriteLine("Line: {0}", error.Line);
            //        output.WriteLine("Position: {0}", error.Column);
            //        output.WriteLine("Message: {0}", error.ErrorText);
            //        output.WriteLine("Error number: {0}", error.ErrorNumber);
            //        output.WriteLine();
            //    }
            //}

            Console.WriteLine(output);
            Console.WriteLine("Run in {0}ms, size {1}ch", stopwatch.ElapsedMilliseconds, output.ToString().Length);
        }
    }
}
