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
using Neptuo.Templates.Compilation.Parsers.Normalization;
using Neptuo.Templates.Compilation.ViewActivators;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Test.Templates.Compilation.CodeGenerators;
using Test.Templates.Compilation.Parsers;
using Test.Templates.Runtime;
using Test.Templates.UI;
using Test.Templates.UI.Converters;
using Test.Templates.UI.Data;
using Test.Templates.UI.Models;
using Test.Templates.Unity;
using IDisposable = Neptuo.IDisposable;

namespace Test.Templates
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






            INameNormalizer componentNormalizer = new CompositeNameNormalizer(
                new SuffixNameNormalizer("Control"),
                new LowerInvariantNameNormalizer()
            );

            INameNormalizer tokenNormalizer = new CompositeNameNormalizer(
                new SuffixNameNormalizer("Extension"),
                new LowerInvariantNameNormalizer()
            );

            IParserRegistry parserRegistry = new DefaultParserRegistry()
                .AddContentBuilder(
                    new ContentBuilderRegistry(componentNormalizer)
                        .AddSearchHandler(e => new GenericContentControlBuilder<GenericContentControl>(c => c.TagName, builderRegistry, builderRegistry))
                )
                .AddTokenBuilder(
                    new TokenBuilderRegistry(tokenNormalizer)
                );






            builderRegistry.LiteralBuilder = new DefaultLiteralControlBuilder<LiteralControl>(c => c.Text);
            builderRegistry.DefaultContentBuilder = new GenericContentControlBuilder<GenericContentControl>(c => c.TagName, builderRegistry, builderRegistry);
            builderRegistry
                .RegisterNamespace("ui", "Test.Templates.UI, Test.Templates")
                .RegisterObserverBuilder<DataContextObserver>("data", "*")
                .RegisterObserverBuilder<VisibleObserver>("ui", "Visible")
                .RegisterHtmlAttributeObserverBuilder<IHtmlAttributeCollectionAware>(c => c.HtmlAttributes)
                .RegisterRootBuilder<GeneratedView>(v => v.Content);

            //XCodeDomGenerator codeGenerator = new XCodeDomGenerator()
            //    .SetStandartGenerators(typeof(GeneratedView), componentManagerDescriptor, typeof(IControl), fieldNameProvider)
            //    //.SetCodeObjectGenerator<ComponentCodeObject>(new CodeDomExtendedComponentObjectGenerator2(fieldNameProvider, componentManagerDescriptor))
            //    .SetCodeObjectGenerator<ComponentCodeObject>(new CodeDomExtendedComponentObjectGenerator(
            //        new CodeDomComponentGenerator(fieldNameProvider, componentManagerDescriptor), 
            //        new CodeDomExtensionObjectGenerator(fieldNameProvider, componentManagerDescriptor)
            //    ))
            //    .SetCodeObjectGenerator<TemplateCodeObject>(new CodeDomTemplateGenerator(fieldNameProvider, componentManagerDescriptor))
            //    .SetCodeObjectGenerator<MethodReferenceCodeObject>(new CodeDomMethodReferenceGenerator());
            IUniqueNameProvider nameProvider = new SequenceUniqueNameProvider("field", 1);

            CodeDomGenerator codeGenerator = new CodeDomGenerator(
                new CodeDomDefaultRegistry()
                    .AddObjectGenerator(
                        new CodeDomObjectGeneratorRegistry()
                            .AddGenerator<CommentCodeObject>(new CodeDomCommentObjectGenerator())
                            .AddGenerator<ComponentCodeObject>(new CodeDomDelegatingObjectGenerator(nameProvider))
                            .AddGenerator<ObserverCodeObject>(new CodeDomObserverObjectGenerator(nameProvider))
                            .AddGenerator<RootCodeObject>(new CodeDomRootObjectGenerator(CodeDomStructureGenerator.Names.EntryPointFieldName))
                            .AddGenerator<LiteralCodeObject>(new CodeDomLiteralObjectGenerator())
                            .AddGenerator<PlainValueCodeObject>(new CodeDomLiteralObjectGenerator())
                    )
                    .AddPropertyGenerator(
                        new CodeDomPropertyGeneratorRegistry()
                            .AddGenerator<SetCodeProperty>(new CodeDomSetPropertyGenerator())
                            .AddGenerator<ListAddCodeProperty>(new CodeDomListAddPropertyGenerator())
                            .AddGenerator<DictionaryAddCodeProperty>(new CodeDomDictionaryAddPropertyGenerator())
                    )
                    .AddStructureGenerator(new CodeDomDefaultStructureGenerator()
                        .SetBaseType<GeneratedView>()
                        .AddInterface<IDisposable>()
                        .SetEntryPointName(CodeDomStructureGenerator.Names.CreateViewPageControlsMethod)
                        .AddEntryPointParameter<GeneratedView>(CodeDomStructureGenerator.Names.EntryPointFieldName)
                    )
                    .AddAttributeGenerator(new CodeDomAttributeGeneratorRegistry()
                        .AddDefaultValueGenerator()
                    )
                    .AddTypeConversionGenerator(new CodeDomDefaultTypeConvertionGenerator())
                    .AddVisitor(new CodeDomVisitorRegistry())
                    .AddDependencyGenerator(new CodeDomDependencyProviderGenerator())
                ,
                new CodeDomDefaultConfiguration()
                    .IsDirectObjectResolve(false)
                    .IsAttributeDefaultEnabled(false)
                    .IsPropertyTypeDefaultEnabled(false)
            );

            CodeCompiler codeCompiler = new CodeCompiler(Environment.CurrentDirectory);
            codeCompiler.IsDebugMode = true;
            codeCompiler.References.AddDirectory(Environment.CurrentDirectory);

            DefaultViewService viewService = new DefaultViewService();
            viewService.ParserService
                .AddContentParser("CodeDom", new XmlContentParser(builderRegistry, builderRegistry.GetLiteralBuilder(), true))
                .AddValueParser("CodeDom", new PlainValueParser())
                .AddValueParser("CodeDom", new TokenValueParser(builderRegistry));

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

            StringWriter output = new StringWriter();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //try
            //{

            IViewServiceContext context = new DefaultViewServiceContext(container);





            //IPropertyInfo propertyInfo = new TypePropertyInfo(typeof(BaseGeneratedView).GetProperty(TypeHelper.PropertyName<BaseGeneratedView>(v => v.Content)));
            //IPropertyDescriptor codeProperty= new ListAddPropertyDescriptor(propertyInfo);
            //StringWriter javascriptOutput = new StringWriter();

            //viewService.ParserService.ProcessContent(File.ReadAllText("Index.html"), new DefaultParserServiceContext(container, contentProperty));
            //viewService.JavascriptGenerator.ProcessTree(contentProperty, new CodeDomGeneratorContext(new DefaultNaming("X", "X", "X", "X"), output, viewService.CodeGeneratorService, container));

            //Console.WriteLine(javascriptOutput);









            //BaseGeneratedView view = (BaseGeneratedView)viewService.ProcessContent("<h:panel class='checkin'><a href='google'>Hello, World!</a></h:panel>", context);

            container.RegisterInstance<ICodeDomNaming>(new CodeDomDefaultNaming("Neptuo.Templates", "Index"));

            ISourceContent content = new DefaultSourceContent(LocalFileSystem.FromFilePath("Index.html").GetContent());
            GeneratedView view = (GeneratedView)viewService.ProcessContent("CodeDom", content, context);
            if (view == null || context.Errors.Any())
            {
                Console.WriteLine("Unnable to compile view...");

                foreach (IErrorInfo errorInfo in context.Errors)
                    Console.WriteLine("{0}:{1} -> {2}", errorInfo.LineNumber, errorInfo.ColumnIndex, errorInfo.ErrorText);
            }
            else
            {
                DebugHelper.Debug("Run", () =>
                {
                    view.Init(container, new ComponentManager());
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
