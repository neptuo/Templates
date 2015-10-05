using Neptuo.Activators;
using Neptuo.Identifiers;
using Neptuo.Models.Features;
using Neptuo.Templates.Compilation;
using Neptuo.Templates.Compilation.CodeGenerators;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers;
using Neptuo.Templates.Compilation.Parsers.Descriptors;
using Neptuo.Templates.Compilation.Parsers.Descriptors.Features;
using Neptuo.Templates.Compilation.Parsers.Normalization;
using Neptuo.Templates.Compilation.Parsers.SyntaxTrees;
using Neptuo.Templates.Compilation.Parsers.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Templates.Compilation.CodeGenerators;
using Test.Templates.UI;

namespace Test.Templates
{
    class TestSyntax
    {
        public static void Test()
        {
            DefaultViewService viewService = new DefaultViewService();
            viewService.AddParserService(CreateParserService());
            viewService.GeneratorService.AddGenerator("CodeDom", CreateCodeGenerator());
            

            ICodeObject codeObject = viewService.ParserService.ProcessContent(
                "Default",
                new DefaultSourceContent("Text {data:Binding Path=ID, Converter=Static} Text {ui:Template Path=~/Test.nt}"),
                new DefaultParserServiceContext(new UnityDependencyContainer())
            );
            Console.WriteLine(codeObject);
        }

        private static IParserService CreateParserService()
        {
            DefaultComponentDescriptor bindingDescriptor = new DefaultComponentDescriptor();
            bindingDescriptor
                .Add<IFieldEnumerator>(new TypePropertyFieldEnumerator(typeof(BindingExtension)))
                .Add<ITypeAware>(new DefaultTypeAware(typeof(BindingExtension)));

            CodeObjectBuilderCollection codeObjectBuilders = new CodeObjectBuilderCollection();
            codeObjectBuilders
                 .Add<SyntaxNodeCollection>(new CollectionCodeObjectBuilder(codeObjectBuilders))
                 .Add<TextSyntax>(new TextCodeObjectBuilder())
                 .Add<CurlySyntax>(new CurlyCodeObjectBuilder(bindingDescriptor));

            CodePropertyBuilderCollection codePropertyBuilders = new CodePropertyBuilderCollection();
            codePropertyBuilders
                .AddSearchHandler(CreateCodePropertyBuilder);

            IParserProvider parserProvider = new DefaultParserCollection()
                .Add<ICodeObjectBuilder>(codeObjectBuilders)
                .Add<ICodePropertyBuilder>(codePropertyBuilders)
                .AddPropertyNormalizer(new LowerInvariantNameNormalizer());

            SyntaxParserService parserService = new SyntaxParserService(parserProvider);
            parserService.Tokenizer
                .Add(new CurlyTokenizer())
                .Add(new AngleTokenizer())
                .Add(new PlainTokenizer());

            parserService.SyntaxBuilders
                .Add(CurlyTokenType.OpenBrace, new CurlySyntaxBuilder())
                .Add(ComposableTokenType.Text, new TextSyntaxBuilder());

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
                    )
                    .AddPropertyGenerator(
                        new CodeDomPropertyGeneratorRegistry()
                            .AddGenerator<SetCodeProperty>(new CodeDomSetPropertyGenerator())
                            .AddGenerator<AddCodeProperty>(new CodeDomAddPropertyGenerator())
                            .AddGenerator<MapCodeProperty>(new CodeDomDictionaryAddPropertyGenerator())
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
