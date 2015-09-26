using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neptuo.Templates.Compilation;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers;
using Neptuo.Templates.Compilation.Parsers.Descriptors;
using Neptuo.Templates.Compilation.Parsers.SyntaxTrees;
using Neptuo.Templates.Compilation.Parsers.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest.Templates.Parsers.Tokenizers;

namespace UnitTest.Templates.Parsers
{
    [TestClass]
    public class Parser_Curly : TestComposableTokenizerBase
    {
        [TestMethod]
        public void Basic()
        {
            CodeObjectBuilderCollection codeObjectBuilders = new CodeObjectBuilderCollection();
            codeObjectBuilders
                 .Add(typeof(SyntaxNodeCollection), new CollectionCodeObjectBuilder(codeObjectBuilders))
                 .Add(typeof(CurlySyntax), new CurlyCodeObjectBuilder(new ComponentDescriptorCollection()));

            CodePropertyBuilderCollection codePropertyBuilders = new CodePropertyBuilderCollection();

            IParserProvider parserProvider = new DefaultParserRegistry()
                .AddRegistry<ICodeObjectBuilder>(codeObjectBuilders)
                .AddRegistry<ICodePropertyBuilder>(codePropertyBuilders);

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
    }
}
