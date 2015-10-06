using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neptuo.Templates.Compilation.Parsers;
using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using System;
using System.Collections.Generic;
using UnitTest.Templates.Parsers.Tokenizers;

namespace UnitTest.Templates.Parsers
{
    [TestClass]
    public class Syntax_Curly : TestComposableTokenizerBase
    {
        [TestMethod]
        public void Basic()
        {
            DefaultTokenizer tokenizer = CreateTokenizer();
            IList<Token> tokens = tokenizer.Tokenize(CreateContentReader("Text {data:Binding Path=ID, Converter=Static} Text {ui:Template Path=~/Test.nt}"), new FakeTokenizerContext());
            //IList<ComposableToken> tokens = tokenizer.Tokenize(CreateContentReader("Text {data:Binding Path=ID} Text"), new FakeTokenizerContext());

            ISyntaxBuilder builder = new SyntaxBuilderCollection()
                .Add(CurlyTokenType.OpenBrace, new CurlySyntaxBuilder())
                .Add(TokenType.Literal, new TextSyntaxBuilder());

            ISyntaxNode node = builder.Build(tokens, 0);

            SyntaxNodeCollection collection = node as SyntaxNodeCollection;
            if (collection != null)
            {
                CurlySyntax curly = collection.Nodes[1] as CurlySyntax;
                if (curly != null)
                {
                    AssertAreEqual(curly.Name.NameToken.Text, "Binding");
                }
            }
        }
    }
}
