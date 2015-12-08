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
    public class Syntax_Curly : TestTokenizerBase
    {
        [TestMethod]
        public void Basic()
        {
            DefaultTokenizer tokenizer = CreateTokenizer();
            IList<Token> tokens = tokenizer.Tokenize(CreateContentReader("Text {data:Binding Path=ID, Converter=Static} Text {ui:Template Path=~/Test.nt}"), new FakeTokenizerContext());
            //IList<ComposableToken> tokens = tokenizer.Tokenize(CreateContentReader("Text {data:Binding Path=ID} Text"), new FakeTokenizerContext());

            ISyntaxNodeFactory builder = new SyntaxNodeBuilderCollection()
                .Add(CurlyTokenType.OpenBrace, new CurlySyntaxNodeBuilder())
                .Add(TokenType.Literal, new LiteralSyntaxNodeBuilder());

            ISyntaxNode node = builder.Create(tokens);

            SyntaxCollection collection = node as SyntaxCollection;
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
