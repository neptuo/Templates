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
            IList<ComposableToken> tokens = CreateTokenizer().Tokenize(CreateContentReader("{Binding Path=ID, Converter=Default}"), new FakeTokenizerContext());

            AssertText(tokens, "{", "Binding", " ", "Path", "=", "ID", ",", " ", "Converter", "=", "Default", "}");
        }

        [TestMethod]
        public void Composable_InValidTokenWithoutCloseBrace()
        {
            IList<ComposableToken> tokens = CreateTokenizer().Tokenize(CreateContentReader("{Binding"), new FakeTokenizerContext());

            AssertText(tokens, "{", "Binding", "}");
            AssertAreEqual(tokens[2].IsVirtual, true);
        }

        [TestMethod]
        public void Composable_InValidTokenWithDoubleOpenBrace()
        {
            IList<ComposableToken> tokens = CreateTokenizer().Tokenize(CreateContentReader("{{Binding"), new FakeTokenizerContext());

            AssertText(tokens, "{", "{", "Binding", "}");
            AssertAreEqual(tokens[0].Type, ComposableTokenType.TokenType.Error);
            AssertAreEqual(tokens[3].IsVirtual, true);
        }

        [TestMethod]
        public void Composable_InValidTokenWithTripleCloseBrace()
        {
            IList<ComposableToken> tokens = CreateTokenizer().Tokenize(CreateContentReader("{Binding}}}"), new FakeTokenizerContext());

            AssertText(tokens, "{", "Binding", "}", "}", "}");
            AssertAreEqual(tokens[3].Type, ComposableTokenType.TokenType.Error);
            AssertAreEqual(tokens[4].Type, ComposableTokenType.TokenType.Error);
        }

        [TestMethod]
        public void Composable_InValidTokenMissingCloseWithNextValid()
        {
            IList<ComposableToken> tokens = CreateTokenizer().Tokenize(CreateContentReader("{Binding as {Binding}"), new FakeTokenizerContext());

            AssertText(tokens, "{", "Binding", " ", "as ", "}", "{", "Binding", "}");
            AssertAreEqual(tokens[4].IsVirtual, true);
        }

        [TestMethod]
        public void Composable_InValidTokenInValidName()
        {
            IList<ComposableToken> tokens = CreateTokenizer().Tokenize(CreateContentReader("{1Binding}"), new FakeTokenizerContext());

            AssertText(tokens, "{", "1Binding", "}");
            AssertAreEqual(tokens[1].Type, ComposableTokenType.TokenType.Error);
        }
    }
}
