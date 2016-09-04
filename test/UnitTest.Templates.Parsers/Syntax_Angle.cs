using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes;
using Neptuo.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest.Templates.Parsers.Tokenizers;

namespace UnitTest.Templates.Parsers
{
    [TestClass]
    public class Syntax_Angle : TestTokenizerBase
    {
        [TestMethod]
        public void Basic()
        {
            DefaultTokenizer tokenizer = CreateTokenizer();
            IList<Token> tokens = tokenizer.Tokenize(CreateContentReader("Text <ui:Literal Text=\"Hello, World!\" />"), new FakeTokenizerContext());
            //IList<ComposableToken> tokens = tokenizer.Tokenize(CreateContentReader("Text {data:Binding Path=ID} Text"), new FakeTokenizerContext());

            INodeFactory builder = new NodeBuilderCollection()
                .Add(AngleTokenType.OpenBrace, new AngleNodeBuilder())
                .Add(TokenType.Literal, new LiteralNodeBuilder());

            INode node = builder.Create(tokens);

            NodeCollection collection = node as NodeCollection;
            if (collection != null)
            {
                AngleNode curly = collection.Nodes[1] as AngleNode;
                if (curly != null)
                {
                    AssertAreEqual(curly.Name.NameToken.Text, "Literal");
                }
            }
        }
    }
}
