﻿using Neptuo;
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
using Neptuo.Templates.Compilation.ViewActivators;
using Neptuo.Templates.Controls;
using Neptuo.Templates.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using TestConsoleNG.Controls;
using TestConsoleNG.Data;
using TestConsoleNG.Extensions;
using TestConsoleNG.Observers;
using TestConsoleNG.SimpleContainer;
using TestConsoleNG.Unity;

namespace TestConsoleNG
{
    public class MyXmlNamespaceManager : XmlNamespaceManager
    {
        public MyXmlNamespaceManager()
            : base(new NameTable())
        { }

        public override string LookupNamespace(string prefix)
        {
            return "template-" + prefix;
        }
    }

    public static class TestTemplates
    {
        private static IDependencyContainer container;

        static TestTemplates()
        {
            container = new UnityDependencyContainer();
            container.RegisterInstance<DataStorage>(new DataStorage(new PersonModel("Jon", "Doe", new AddressModel("Dlouhá street", 23, "Prague", 10001))));
            container.RegisterInstance<IValueConverterService>(new ValueConverterService().SetConverter("NullToBool", new NullToBoolValueConverter()));
            container.RegisterType<IViewServiceContext, DefaultViewServiceContext>();
        }

        public static void Test()
        {
//            string xml = @"<?xml version='1.0'?>
//<div><x:root></x:root></div>";

//            XmlNamespaceManager mgr = new MyXmlNamespaceManager();

//            XmlParserContext ctx = new XmlParserContext(null, mgr, null, XmlSpace.Preserve);
//            using (XmlReader reader = XmlReader.Create(new StringReader(xml), null, ctx))
//            {
//                XDocument document = XDocument.Load(reader);
//                Console.WriteLine(document);
//                //XmlDocument doc = new XmlDocument();
//                //doc.Load(reader);
//            }







            TypeBuilderRegistry builderRegistry = new TypeBuilderRegistry(
                new TypeBuilderRegistryConfiguration(container)//.AddComponentSuffix("presenter"),
            );
            builderRegistry.LiteralBuilder = new DefaultLiteralControlBuilder<LiteralControl>(c => c.Text);
            builderRegistry.GenericContentBuilder = new TypeScanner.TransientComponentBuilder(() => new GenericContentControlBuilder<GenericContentControl>(c => c.TagName, builderRegistry, builderRegistry));
            builderRegistry
                .RegisterDefaultNamespace("TestConsoleNG.Extensions, TestConsoleNG.Components")
                .RegisterNamespace("h", "TestConsoleNG.Controls, TestConsoleNG.Components")
                .RegisterObserverBuilder("data", "*", new DefaultTypeObserverBuilder(typeof(DataContextObserver), builderRegistry))
                .RegisterObserverBuilder("ui", "Visible", new DefaultTypeObserverBuilder(typeof(VisibleObserver), builderRegistry))
                .RegisterObserverBuilder(null, "*", new HtmlAttributeObserverBuilder())
                .RegisterPropertyBuilder<string>(new StringPropertyBuilder())
                .RegisterPropertyBuilder<ITemplate>(new TemplatePropertyBuilder())
                .RegisterRootBuilder(new RootContentBuilder(builderRegistry, builderRegistry));

            IFieldNameProvider fieldNameProvider = new SequenceFieldNameProvider();
            CodeDomGenerator codeGenerator = new CodeDomGenerator()
                .SetStandartGenerators(fieldNameProvider)
                .SetCodeObjectGenerator<TemplateCodeObject>(new CodeDomTemplateGenerator(fieldNameProvider))
                .SetCodeObjectGenerator<MethodReferenceCodeObject>(new CodeDomMethodReferenceGenerator());

            CodeCompiler codeCompiler = new CodeCompiler(Environment.CurrentDirectory);
            codeCompiler.IsDebugMode = true;
            codeCompiler.References.AddDirectory(Environment.CurrentDirectory);

            DefaultViewService viewService = new DefaultViewService();
            viewService.ParserService.ContentParsers.Add(new XmlContentParser(builderRegistry, builderRegistry.GetLiteralBuilder()));
            viewService.ParserService.DefaultValueParser = new PlainValueParser();
            viewService.ParserService.ValueParsers.Add(new TokenValueParser(builderRegistry));
            viewService.GeneratorService.AddGenerator("CodeDom", codeGenerator);
            viewService.ActivatorService.AddActivator("CodeDom", new NullViewActivator());
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

            container.RegisterInstance<INaming>(new DefaultNaming("Index.cs", CodeDomStructureGenerator.Names.CodeNamespace, "Index", "Index.dll"));

            ISourceContent content = new DefaultSourceContent(LocalFileSystem.FromFilePath("Index.html").GetContent());
            GeneratedView view = (GeneratedView)viewService.ProcessContent("CodeDom", content, context);
            if (view == null || context.Errors.Any())
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
