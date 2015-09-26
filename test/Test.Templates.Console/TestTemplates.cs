using Neptuo;
using Neptuo.Activators;
using Neptuo.Compilers;
using Neptuo.Compilers.Errors;
using Neptuo.Diagnostics;
using Neptuo.Identifiers;
using Neptuo.Linq.Expressions;
using Neptuo.Templates;
using Neptuo.Templates.Compilation;
using Neptuo.Templates.Compilation.AssemblyScanning;
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
            container.Definitions
                .AddScoped<DataStorage>(container.ScopeName, new DataStorage(new PersonModel("Jon", "Doe", new AddressModel("Dlouhá street", 23, "Prague", 10001))))
                .AddScoped<IValueConverterService>(container.ScopeName, new ValueConverterService().SetConverter("NullToBool", new NullToBoolValueConverter()))
                .AddScoped<IViewServiceContext, DefaultViewServiceContext>(container.ScopeName);
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


            // Name normalizer for components/controls.
            INameNormalizer componentNormalizer = new CompositeNameNormalizer(
                new SuffixNameNormalizer("Control"),
                new LowerInvariantNameNormalizer()
            );

            INameNormalizer observerNormalizer = new CompositeNameNormalizer(
                new SuffixNameNormalizer("Observer"),
                new LowerInvariantNameNormalizer()
            );

            // Name normalizer for tokens.
            INameNormalizer tokenNormalizer = new CompositeNameNormalizer(
                new SuffixNameNormalizer("Extension"),
                new LowerInvariantNameNormalizer()
            );

            // Create extensible parser registry.
            IParserProvider parserRegistry = new DefaultParserRegistry()
                .AddPropertyNormalizer(new LowerInvariantNameNormalizer())
                .AddTypeScanner(
                    new TypeScanner()
                        .AddTypeFilterNotAbstract()
                        .AddTypeFilterNotInterface()
                        .AddAssembly("ui", "Test.Templates.UI", "Test.Templates")
                        .AddEmptyPrefix("data", "Observers")
                )
                .AddContentBuilderRegistry(
                    new ContentBuilderRegistry(componentNormalizer)
                        .AddGenericControlSearchHandler<GenericContentControl>(c => c.TagName)
                        .AddRootBuilder<GeneratedView>(v => v.Content)
                )
                .AddObserverBuilder(
                    new ObserverBuilderRegistry(observerNormalizer)
                        .AddHtmlAttributeBuilder<IHtmlAttributeCollectionAware>(c => c.HtmlAttributes)
                        .AddBuilder<VisibleObserver>("ui", "visible")
                        .AddBuilder<DataContextObserver>("data", "*")
                )
                .AddPropertyBuilder(
                    new ContentPropertyBuilderRegistry()
                        .AddSearchHandler(propertyInfo => new TypeDefaultPropertyBuilder())
                )
                .AddLiteralBuilder(new LiteralBuilder())
                .AddTokenBuilder(new TokenBuilderRegistry(tokenNormalizer))
                .RunTypeScanner();


            // Create code generator.
            IUniqueNameProvider nameProvider = new SequenceUniqueNameProvider("field", 1);
            CodeDomGenerator codeGenerator = new CodeDomGenerator(
                new CodeDomDefaultRegistry()
                    .AddObjectGenerator(
                        new CodeDomObjectGeneratorRegistry()
                            .AddGenerator<CommentCodeObject>(new CodeDomCommentObjectGenerator())
                            .AddGenerator<XComponentCodeObject>(new CodeDomDelegatingObjectGenerator(nameProvider))
                            .AddGenerator<ObserverCodeObject>(new CodeDomObserverObjectGenerator(nameProvider))
                            .AddGenerator<RootCodeObject>(new CodeDomRootObjectGenerator(CodeDomStructureGenerator.Names.EntryPointFieldName))
                            .AddGenerator<PlainValueCodeObject>(new CodeDomLiteralObjectGenerator())
                    )
                    .AddPropertyGenerator(
                        new CodeDomPropertyGeneratorRegistry()
                            .AddGenerator<XSetCodeProperty>(new CodeDomSetPropertyGenerator())
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

            CodeCompiler codeCompiler = new CodeCompiler();
            codeCompiler.AddTempDirectory(Environment.CurrentDirectory);
            codeCompiler.AddIsDebugMode(true);
            codeCompiler.References().AddDirectory(Environment.CurrentDirectory);

            DefaultViewService viewService = new DefaultViewService();
            viewService.AddParserService(new TextParserService()
                .AddContentParser("Default", new TextXmlContentParser(parserRegistry, true))
                .AddValueParser("Default", new TextPlainValueParser())
                .AddValueParser("Default", new TextTokenValueParser(parserRegistry))
            );

            viewService.GeneratorService.AddGenerator("CodeDom", codeGenerator);
            viewService.ActivatorService.AddActivator("CodeDom", new NullViewActivator());
            viewService.CompilerService.AddCompiler("CodeDom", codeCompiler);

            viewService.CompilerService.AddCompiler("SharpKit", new SharpKitCodeCompiler());

            viewService.Pipeline.AddParserService("CodeDom", "Default");
            viewService.Pipeline.AddParserService("SharpKit", "Default");
            viewService.Pipeline.AddCodeGeneratorService("SharpKit", "CodeDom");
            viewService.Pipeline.AddViewActivatorService("SharpKit", "CodeDom");

            // Register view service to the container.
            container.Definitions.AddScoped<IViewService>(container.ScopeName, viewService);

            StringWriter output = new StringWriter();

            IViewServiceContext context = new DefaultViewServiceContext(container);

            container.Definitions.AddScoped<ICodeDomNaming>(container.ScopeName, new CodeDomDefaultNaming("Neptuo.Templates", "Index"));

            ISourceContent content = new DefaultSourceContent(File.ReadAllText("Index.html"));
            DebugHelper.Debug("Execute", () =>
            {
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

                Console.WriteLine(output);
                Console.WriteLine("Output size {0}ch", output.ToString().Length);
            });

            string javascriptCode = (string)viewService.ProcessContent("SharpKit", content, context);
            Console.WriteLine(javascriptCode);
        }
    }
}
