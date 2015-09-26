using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neptuo.Models.Features;
using Neptuo.Templates.Compilation;
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
using UnitTest.Templates.Parsers.Extensions;
using UnitTest.Templates.Parsers.Tokenizers;

namespace UnitTest.Templates.Parsers
{
    [TestClass]
    public class Parser_Curly : TestComposableTokenizerBase
    {
        [TestMethod]
        public void Basic()
        {
            DefaultComponentDescriptor bindingDescriptor = new DefaultComponentDescriptor();
            bindingDescriptor
                .Add<IFieldEnumerator>(new TypePropertyFieldEnumerator(typeof(BindingExtension)));

            CodeObjectBuilderCollection codeObjectBuilders = new CodeObjectBuilderCollection();
            codeObjectBuilders
                 .Add(typeof(SyntaxNodeCollection), new CollectionCodeObjectBuilder(codeObjectBuilders))
                 .Add(typeof(TextSyntax), new TextCodeObjectBuilder())
                 .Add(typeof(CurlySyntax), new CurlyCodeObjectBuilder(bindingDescriptor));

            CodePropertyBuilderCollection codePropertyBuilders = new CodePropertyBuilderCollection();
            codePropertyBuilders
                .AddSearchHandler(CreateCodePropertyBuilder);

            IParserProvider parserProvider = new DefaultParserRegistry()
                .AddRegistry<ICodeObjectBuilder>(codeObjectBuilders)
                .AddRegistry<ICodePropertyBuilder>(codePropertyBuilders)
                .AddPropertyNormalizer(new LowerInvariantNameNormalizer());

            SyntaxParserService parserService = new SyntaxParserService(parserProvider);
            parserService.Tokenizer
                .Add(new CurlyTokenizer())
                .Add(new AngleTokenizer())
                .Add(new PlainTokenizer());

            parserService.SyntaxBuilders
                .Add(CurlyTokenType.OpenBrace, new CurlySyntaxBuilder())
                .Add(ComposableTokenType.Text, new TextSyntaxBuilder());

            ICodeObject codeObject = parserService.ProcessContent(
                "Default", 
                new DefaultSourceContent("Text {data:Binding Path=ID, Converter=Static} Text {ui:Template Path=~/Test.nt}"), 
                new DefaultParserServiceContext(new FakeDependencyProvider())
            );
        }

        private bool CreateCodePropertyBuilder(Type propertyType, out ICodePropertyBuilder builder)
        {
            builder = new DefaultCodePropertyBuilder();
            return true;
        }
    }
}
