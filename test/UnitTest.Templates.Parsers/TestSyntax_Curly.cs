using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest.Templates.Parsers.Tokenizers;
using Neptuo.Templates.Compilation.Parsers.Tokenizers;
using System.Collections.Generic;
using Neptuo.Templates.Compilation.Parsers;

namespace UnitTest.Templates.Parsers
{
    [TestClass]
    public class TestSyntax_Curly : TestComposableTokenizerBase
    {
        [TestMethod]
        public void Syntax_Curly_BasicSyntax()
        {
            ComposableTokenizer tokenizer = CreateTokenizer();
            IList<ComposableToken> tokens = tokenizer.Tokenize(CreateContentReader("Text {data:Binding} Text"), new FakeTokenizerContext());

            ISyntaxBuilder builder = new SyntaxBuilderCollection()
                .Add(CurlyTokenType.OpenBrace, new CurlySyntaxBuilder())
                .Add(ComposableTokenType.Text, new TextSyntaxBuilder());

            ISyntaxNode node = builder.Build(tokens, 0);
            Console.WriteLine(node);
        }
    }
}
