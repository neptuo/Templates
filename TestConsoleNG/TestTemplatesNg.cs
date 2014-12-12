using Neptuo;
using Neptuo.ComponentModel;
using Neptuo.Diagnostics;
using Neptuo.FileSystems;
using Neptuo.Linq.Expressions;
using Neptuo.Templates;
using Neptuo.Templates.Compilation;
using Neptuo.Templates.Compilation.CodeCompilers;
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
    public static class TestTemplatesNg
    {
        private static IDependencyContainer container;

        static TestTemplatesNg()
        {
            container = new UnityDependencyContainer();
            container.RegisterInstance<IFileProvider>(new FileProvider(LocalFileSystem.FromDirectoryPath(Environment.CurrentDirectory)));
            container.RegisterInstance<DataStorage>(new DataStorage(new PersonModel("Jon", "Doe", new AddressModel("Dlouhá street", 23, "Prague", 10001))));
            container.RegisterInstance<IValueConverterService>(new ValueConverterService().SetConverter("NullToBool", new NullToBoolValueConverter()));
            container.RegisterType<IViewServiceContext, DefaultViewServiceContext>();
        }

        public static void Test()
        {
            TypeBuilderRegistry builderRegistry = new TypeBuilderRegistry(
                new TypeBuilderRegistryConfiguration(container),//.AddComponentSuffix("presenter"),
                new DefaultLiteralControlBuilderFactory<LiteralControl>(c => c.Text),
                new FuncContentBuilderFactory((prefix, name) => new GenericContentControlBuilder<GenericContentControl>(c => c.TagName))
            );
            builderRegistry.RegisterNamespace(new NamespaceDeclaration("h", "TestConsoleNG.Controls, TestConsoleNG.Components"));
            builderRegistry.RegisterNamespace(new NamespaceDeclaration(null, "TestConsoleNG.Extensions, TestConsoleNG.Components"));
            builderRegistry.RegisterObserverBuilder("data", "*", new DefaultTypeObserverBuilderFactory(typeof(DataContextObserver)));
            builderRegistry.RegisterObserverBuilder("ui", "Visible", new DefaultTypeObserverBuilderFactory(typeof(VisibleObserver)));
            builderRegistry.RegisterPropertyBuilder(typeof(string), new DefaultPropertyBuilderFactory<StringPropertyBuilder>());
            builderRegistry.RegisterPropertyBuilder(typeof(ITemplate), new DefaultPropertyBuilderFactory<TemplatePropertyBuilder>());
            builderRegistry.RegisterComponentBuilder(null, "NeptuoTemplatesRoot", new FuncContentBuilderFactory((prefix, name) => new RootContentBuilder()));

            IFieldNameProvider fieldNameProvider = new SequenceFieldNameProvider();
            CodeDomGenerator codeGenerator = new CodeDomGenerator();
            codeGenerator.RegisterStandartCodeGenerators(fieldNameProvider);
            codeGenerator.SetCodeObjectGenerator(typeof(TemplateCodeObject), new CodeDomTemplateGenerator(fieldNameProvider));
            codeGenerator.SetCodeObjectGenerator(typeof(MethodReferenceCodeObject), new CodeDomMethodReferenceGenerator());

            CodeCompiler codeCompiler = new CodeCompiler(Environment.CurrentDirectory);
            codeCompiler.References.AddDirectory(Environment.CurrentDirectory);

            DefaultViewService viewService = new DefaultViewService();

            viewService.ParserService.ContentParsers.Add(new XmlContentParser(builderRegistry));
            viewService.ParserService.DefaultValueParser = new PlainValueParser();
            viewService.ParserService.ValueParsers.Add(new TokenValueParser(builderRegistry));
            viewService.GeneratorService.AddGenerator("CodeDom", codeGenerator);
            viewService.ActivatorService.AddActivator("CodeDom", codeCompiler);
            viewService.CompilerService.AddCompiler("CodeDom", codeCompiler);





            //CodeDomViewService viewService = new CodeDomViewService(true);
            //viewService.DebugMode = CodeDomDebugMode.AlwaysReGenerate | CodeDomDebugMode.GenerateSourceCode;
            //viewService.ParserService.ContentParsers.Add(new XmlContentParser(builderRegistry));
            //viewService.ParserService.DefaultValueParser = new PlainValueParser();
            //viewService.ParserService.ValueParsers.Add(new TokenValueParser(builderRegistry));
            //viewService.NamingService = new HashNamingService(new FileProvider(LocalFileSystem.FromDirectoryPath(Environment.CurrentDirectory)));

            container.RegisterInstance<IViewService>(viewService);
            container.RegisterInstance<IComponentManager>(new ComponentManager());

            StringWriter output = new StringWriter();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //try
            //{

            IViewServiceContext context = new DefaultViewServiceContext(container);





            //IPropertyInfo propertyInfo = new TypePropertyInfo(typeof(BaseGeneratedView).GetProperty(TypeHelper.PropertyName<BaseGeneratedView>(v => v.Content)));
            //IPropertyDescriptor contentProperty= new ListAddPropertyDescriptor(propertyInfo);
            //StringWriter javascriptOutput = new StringWriter();

            //viewService.ParserService.ProcessContent(File.ReadAllText("Index.html"), new DefaultParserServiceContext(container, contentProperty));
            //viewService.JavascriptGenerator.ProcessTree(contentProperty, new CodeDomGeneratorContext(new DefaultNaming("X", "X", "X", "X"), output, viewService.CodeGeneratorService, container));

            //Console.WriteLine(javascriptOutput);









            //BaseGeneratedView view = (BaseGeneratedView)viewService.ProcessContent("<h:panel class='checkin'><a href='google'>Hello, World!</a></h:panel>", context);

            container.RegisterInstance<INaming>(new HashNamingService(new FileProvider(LocalFileSystem.FromDirectoryPath(Environment.CurrentDirectory))).FromFile("Index.html"));

            ISourceContent content = new DefaultSourceContent(LocalFileSystem.FromFilePath("Index.html").GetContent());
            GeneratedView view = (GeneratedView)viewService.ProcessContent("CodeDom", content, context);
            if (view == null)
            {
                Console.WriteLine("Unable to compile view...");

                foreach (IErrorInfo errorInfo in context.Errors)
                    Console.WriteLine("{0}:{1} -> {2}", errorInfo.LineNumber, errorInfo.ColumnIndex, errorInfo.ErrorText);
            }
            else
            {
                DebugHelper.Debug("Run", () =>
                {
                    view.Setup(new ViewPage(container.Resolve<IComponentManager>()), container.Resolve<IComponentManager>(), container);
                    view.CreateControls();
                    view.Init();
                    view.Render(new HtmlTextWriter(output));
                    view.Dispose();
                });

            }
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
