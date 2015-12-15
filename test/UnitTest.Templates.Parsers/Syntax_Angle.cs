using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
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

            ISyntaxNodeFactory builder = new SyntaxNodeBuilderCollection()
                .Add(AngleTokenType.OpenBrace, new AngleSyntaxNodeBuilder())
                .Add(TokenType.Literal, new LiteralSyntaxNodeBuilder());

            ISyntaxNode node = builder.Create(tokens);

            SyntaxCollection collection = node as SyntaxCollection;
            if (collection != null)
            {
                AngleSyntax curly = collection.Nodes[1] as AngleSyntax;
                if (curly != null)
                {
                    AssertAreEqual(curly.Name.NameToken.Text, "Literal");
                }
            }
        }
    }
}
