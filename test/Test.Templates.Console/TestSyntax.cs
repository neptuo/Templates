using Neptuo.Activators;
using Neptuo.Compilers;
using Neptuo.Compilers.Errors;
using Neptuo.Diagnostics;
using Neptuo.Identifiers;
using Neptuo.Models.Features;
using Neptuo.Templates.Compilation;
using Neptuo.Templates.Compilation.CodeCompilers;
using Neptuo.Templates.Compilation.CodeGenerators;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers;
using Neptuo.Templates.Compilation.Parsers.Descriptors;
using Neptuo.Templates.Compilation.Parsers.Descriptors.Features;
using Neptuo.Templates.Compilation.Parsers.Normalization;
using Neptuo.Templates.Compilation.Parsers.Syntax;
using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using Neptuo.Templates.Compilation.ViewActivators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Templates.Compilation.CodeGenerators;
using Test.Templates.Runtime;
using Test.Templates.UI;
using Test.Templates.UI.Converters;
using Test.Templates.UI.Data;
using Test.Templates.UI.Models;

namespace Test.Templates
{
    class TestSyntax
    {
        private static IDependencyContainer container;

        static TestSyntax()
        {
            container = new SimpleDependencyContainer();
            container.Definitions
                .AddScoped<DataStorage>(container.ScopeName, new DataStorage(new PersonModel("Jon", "Doe", new AddressModel("Dlouhá street", "23", "Prague", "10001"))))
                .AddScoped<IValueConverterService>(container.ScopeName, new ValueConverterService().SetConverter("NullToBool", new NullToBoolValueConverter()))
                .AddScoped<IViewServiceContext, DefaultViewServiceContext>(container.ScopeName);
        }

        public static void Test()
        {
            DefaultViewService viewService = new DefaultViewService();
            viewService.AddParserService(CreateParserService());
            
            //ICodeObject codeObject = viewService.ParserService.ProcessContent(
            //    "Default",
            //    new DefaultSourceContent("Text {data:Binding Path=ID, Converter=Static} Text {ui:Template Path=~/Test.nt}"),
            //    new DefaultParserServiceContext(new UnityDependencyContainer())
            //);
            //Console.WriteLine(codeObject);


            CodeCompiler codeCompiler = new CodeCompiler();
            codeCompiler.AddTempDirectory(Environment.CurrentDirectory);
            codeCompiler.AddIsDebugMode(true);
            codeCompiler.References().AddDirectory(Environment.CurrentDirectory);

            viewService.GeneratorService.AddGenerator("CodeDom", CreateCodeGenerator());
            viewService.ActivatorService.AddActivator("CodeDom", new NullViewActivator());
            viewService.CompilerService.AddCompiler("CodeDom", codeCompiler);

            viewService.CompilerService.AddCompiler("SharpKit", new SharpKitCodeCompiler());

            viewService.Pipeline.AddParserService("CodeDom", "Default");
            viewService.Pipeline.AddParserService("SharpKit", "Default");
            viewService.Pipeline.AddCodeGeneratorService("SharpKit", "CodeDom");
            viewService.Pipeline.AddViewActivatorService("SharpKit", "CodeDom");



            StringWriter output = new StringWriter();

            IViewServiceContext context = new DefaultViewServiceContext(container);

            container.Definitions.AddScoped<ICodeDomNaming>(container.ScopeName, new CodeDomDefaultNaming("Neptuo.Templates", "SyntaxIndex"));

            ISourceContent content = new DefaultSourceContent("FirstName: {data:Binding Path=Firstname}, LastName: {data:Binding Path=Lastname}");
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

                    Console.WriteLine(output);
                    Console.WriteLine("Output size {0}ch", output.ToString().Length);

                    //string javascriptCode = (string)viewService.ProcessContent("SharpKit", content, context);
                    //Console.WriteLine(javascriptCode);
                }
            });
        }

        private static IParserService CreateParserService()
        {
            DefaultComponentDescriptor bindingDescriptor = new DefaultComponentDescriptor();
            bindingDescriptor
                .Add<IName>(new DefaultName("data", "Binding"))
                .Add<ITypeAware>(new DefaultTypeAware(typeof(BindingExtension)))
                .Add<IFieldEnumerator>(new TypePropertyFieldEnumerator(typeof(BindingExtension)));

            CodeObjectBuilderCollection codeObjectBuilders = new CodeObjectBuilderCollection();
            codeObjectBuilders
                 .Add<SyntaxCollection>(new CollectionCodeObjectBuilder(codeObjectBuilders))
                 .Add<LiteralSyntax>(new TextCodeObjectBuilder())
                 .Add<CurlySyntax>(new CurlyCodeObjectBuilder(bindingDescriptor));

            CodePropertyBuilderCollection codePropertyBuilders = new CodePropertyBuilderCollection();
            codePropertyBuilders
                .AddSearchHandler(CreateCodePropertyBuilder);

            IParserProvider parserProvider = new DefaultParserCollection()
                .Add<ICodeObjectBuilder>(codeObjectBuilders)
                .Add<ICodePropertyBuilder>(codePropertyBuilders)
                .AddPropertyNormalizer(new LowerInvariantNameNormalizer());

            SyntaxParserService parserService = new SyntaxParserService(parserProvider);
            parserService.ContentTokenizer
                .Add(new CurlyTokenBuilder())
                .Add(new AngleTokenBuilder())
                .Add(new LiteralTokenBuilder());

            parserService.SyntaxBuilders
                .Add(CurlyTokenType.OpenBrace, new CurlySyntaxNodeBuilder())
                .Add(TokenType.Literal, new LiteralSyntaxNodeBuilder());

            return parserService;
        }

        private static bool CreateCodePropertyBuilder(Type propertyType, out ICodePropertyBuilder builder)
        {
            builder = new DefaultCodePropertyBuilder();
            return true;
        }

        private static ICodeGenerator CreateCodeGenerator()
        {
            // Create code generator.
            IUniqueNameProvider nameProvider = new SequenceUniqueNameProvider("field", 1);
            CodeDomGenerator codeGenerator = new CodeDomGenerator(
                new CodeDomDefaultRegistry()
                    .AddObjectGenerator(
                        new CodeDomObjectGeneratorRegistry()
                            .AddGenerator<CommentCodeObject>(new CodeDomCommentObjectGenerator())
                            .AddGenerator<ComponentCodeObject>(new CodeDomDelegatingObjectGenerator(nameProvider))
                            .AddGenerator<LiteralCodeObject>(new CodeDomLiteralObjectGenerator())
                            .AddGenerator<CodeObjectCollection>(new CodeDomObjectCollectionGenerator(typeof(GeneratedView), "view", "Content"))
                    )
                    .AddPropertyGenerator(
                        new CodeDomPropertyGeneratorRegistry()
                            .AddGenerator<SetCodeProperty>(new CodeDomSetPropertyGenerator())
                            .AddGenerator<AddCodeProperty>(new CodeDomAddPropertyGenerator())
                            .AddGenerator<MapCodeProperty>(new CodeDomMapPropertyGenerator())
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

            return codeGenerator;
        }
    }
}
