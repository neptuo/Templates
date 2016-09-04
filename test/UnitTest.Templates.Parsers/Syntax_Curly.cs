using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neptuo.Templates.Compilation.Parsers;
using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes;
using Neptuo.Text;
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
            ITokenizer tokenizer = CreateTokenizer();
            IList<Token> tokens = tokenizer.Tokenize(CreateContentReader("Text {data:Binding Path=ID, Converter=Static} Text {ui:Template Path=~/Test.nt}"), new FakeTokenizerContext());
            //IList<ComposableToken> tokens = tokenizer.Tokenize(CreateContentReader("Text {data:Binding Path=ID} Text"), new FakeTokenizerContext());

            INodeFactory builder = new NodeBuilderCollection()
                .Add(CurlyTokenType.OpenBrace, new CurlyNodeBuilder())
                .Add(TokenType.Literal, new LiteralNodeBuilder());

            INode node = builder.Create(tokens);

            NodeCollection collection = node as NodeCollection;
            if (collection != null)
            {
                CurlyNode curly = collection.Nodes[1] as CurlyNode;
                if (curly != null)
                {
                    AssertAreEqual(curly.Name.NameToken.Text, "Binding");
                }
            }
        }
    }
}
