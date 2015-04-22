using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neptuo.Templates.Compilation.Parsers.Tokenizers;
using Neptuo.Templates.Compilation.Parsers.Tokenizers.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Templates.Parsers.Tokenizers
{
    [TestClass]
    public class TestComposableTokenizer : TestComposableTokenizerBase
    {
        [TestMethod]
        public void Composable_ValidToken()
        {
            IList<ComposableToken> tokens = CreateTokenizer().Tokenize(CreateContentReader("{Binding}"), new FakeTokenizerContext());

            AssertLength(tokens, 3);
            AssertText(tokens, "{", "Binding", "}");
            AssertContentInfo(tokens,
                CreateContentInfo(0, 1),
                CreateContentInfo(1, 7),
                CreateContentInfo(8, 1)
            );
        }

        [TestMethod]
        public void Composable_ValidTokenWithText()
        {
            IList<ComposableToken> tokens = CreateTokenizer().Tokenize(CreateContentReader("abc {Binding}  subtraction(); adaptation(); "), new FakeTokenizerContext());

            AssertLength(tokens, 5);
            AssertText(tokens, "abc ", "{", "Binding", "}", "  subtraction(); adaptation(); ");
        }

        [TestMethod]
        public void Composable_ValidTokenWithDefaultAttributes()
        {
            IList<ComposableToken> tokens = CreateTokenizer().Tokenize(CreateContentReader("{Binding ID,Name,Price}"), new FakeTokenizerContext());

            AssertText(tokens, "{", "Binding", " ", "ID", ",", "Name", ",", "Price", "}");
        }

        [TestMethod]
        public void Composable_ValidTokenWithNamedAttributes()
        {
            IList<ComposableToken> tokens = CreateTokenizer().Tokenize(CreateContentReader("{Binding Path=ID}"), new FakeTokenizerContext());

            AssertText(tokens, "{", "Binding", " ", "Path", "=", "ID", "}");
        }

        [TestMethod]
        public void Composable_InValidTokenMissingCloseWithNextValid()
        {
            IList<ComposableToken> tokens = CreateTokenizer().Tokenize(CreateContentReader("{Binding as {Binding}"), new FakeTokenizerContext());

            AssertLength(tokens, 2);
            AssertText(tokens, "{", "Binding as {Binding}");
        }
    }
}
