using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest.Templates.Parsers.Tokenizers;
using Neptuo.Templates.Compilation.Parsers.Tokenizers;
using System.Collections.Generic;
using Neptuo.Templates.Compilation.Parsers;

namespace UnitTest.Templates.Parsers
{
    [TestClass]
    public class Syntax_Curly : TestComposableTokenizerBase
    {
        [TestMethod]
        public void Basic()
        {
            ComposableTokenizer tokenizer = CreateTokenizer();
            IList<ComposableToken> tokens = tokenizer.Tokenize(CreateContentReader("Text {data:Binding} Text"), new FakeTokenizerContext());

            ISyntaxBuilder builder = new SyntaxBuilderCollection()
                .Add(CurlyTokenType.OpenBrace, new CurlySyntaxBuilder())
                .Add(ComposableTokenType.Text, new TextSyntaxBuilder());

            ISyntaxNode node = builder.Build(tokens, 0);

            SyntaxtCollection collection = node as SyntaxtCollection;
            if (collection != null)
            {
                CurlySyntax curly = collection.Nodes[1] as CurlySyntax;
                if (curly != null)
                {
                    AssertAreEqual(curly.Name.Name.Text, "Binding");
                }
            }
        }
    }
}
