using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Templates.Parsers.Tokenizers
{
    [TestClass]
    public class TestTokenizer_Curly : TestTokenizerBase
    {
        [TestMethod]
        public void Curly_ValidToken()
        {
            IList<Token> tokens = CreateTokenizer().Tokenize(CreateContentReader("{Binding}"), new FakeTokenizerContext());

            AssertLength(tokens, 3);
            AssertTokens(tokens, "{", "Binding", "}");
            AssertContentInfo(tokens,
                CreateContentInfo(0, 1),
                CreateContentInfo(1, 7),
                CreateContentInfo(8, 1)
            );
        }

        [TestMethod]
        public void Curly_ValidWithPrefixToken()
        {
            IList<Token> tokens = CreateTokenizer().Tokenize(CreateContentReader("{data:Binding}"), new FakeTokenizerContext());

            AssertTokens(tokens, "{", "data", ":", "Binding", "}");
            AssertContentInfo(tokens,
                CreateContentInfo(0, 1),
                CreateContentInfo(1, 4),
                CreateContentInfo(5, 1),
                CreateContentInfo(6, 7),
                CreateContentInfo(13, 1)
            );
        }

        [TestMethod]
        public void Curly_ValidTokenWithText()
        {
            IList<Token> tokens = CreateTokenizer().Tokenize(CreateContentReader("abc {Binding}  subtraction(); adaptation(); "), new FakeTokenizerContext());

            AssertLength(tokens, 5);
            AssertTokens(tokens, "abc ", "{", "Binding", "}", "  subtraction(); adaptation(); ");
        }

        [TestMethod]
        public void Curly_ValidTokenWithDefaultAttributes()
        {
            IList<Token> tokens = CreateTokenizer().Tokenize(CreateContentReader("{Binding ID,Name,Price}"), new FakeTokenizerContext());

            AssertTokens(tokens, "{", "Binding", " ", "ID", ",", "Name", ",", "Price", "}");
        }

        [TestMethod]
        public void Curly_ValidTokenWithNamedAttributes()
        {
            IList<Token> tokens = CreateTokenizer().Tokenize(CreateContentReader("{Binding Path=ID, Converter=Default}"), new FakeTokenizerContext());

            AssertTokens(tokens, "{", "Binding", " ", "Path", "=", "ID", ",", " ", "Converter", "=", "Default", "}");
        }

        [TestMethod]
        public void Curly_ValidTokenWithDefaultAndNamedAttributes()
        {
            IList<Token> tokens = CreateTokenizer().Tokenize(CreateContentReader("{Binding ID, Converter=Default}"), new FakeTokenizerContext());

            AssertTokens(tokens, "{", "Binding", " ", "ID", ",", " ", "Converter", "=", "Default", "}");
        }

        [TestMethod]
        public void Curly_InValidTokenWithoutCloseBrace()
        {
            IList<Token> tokens = CreateTokenizer().Tokenize(CreateContentReader("{Binding"), new FakeTokenizerContext());

            AssertTokens(tokens, "{", "Binding", "}");
            AssertAreEqual(tokens[2].IsVirtual, true);
        }

        [TestMethod]
        public void Curly_InValidTokenWithDoubleOpenBrace()
        {
            IList<Token> tokens = CreateTokenizer().Tokenize(CreateContentReader("{{Binding"), new FakeTokenizerContext());

            AssertTokens(tokens, "{", "{", "Binding", "}");
            AssertAreEqual(tokens[0].IsSkipped, true);
            AssertAreEqual(tokens[3].IsVirtual, true);
        }

        [TestMethod]
        public void Curly_InValidTokenWithTripleCloseBrace()
        {
            IList<Token> tokens = CreateTokenizer().Tokenize(CreateContentReader("{Binding}}}"), new FakeTokenizerContext());

            AssertTokens(tokens, "{", "Binding", "}", "}", "}");
            AssertAreEqual(tokens[3].IsSkipped, true);
            AssertAreEqual(tokens[4].IsSkipped, true);
        }

        [TestMethod]
        public void Curly_InValidTokenMissingCloseWithNextValid()
        {
            IList<Token> tokens = CreateTokenizer().Tokenize(CreateContentReader("{Binding as {Binding}"), new FakeTokenizerContext());

            AssertTokens(tokens, "{", "Binding", " ", "as ", "}", "{", "Binding", "}");
            AssertAreEqual(tokens[4].IsVirtual, true);
        }

        [TestMethod]
        public void Curly_InValidTokenMissingOneCloseWhenTwoRequired()
        {
            IList<Token> tokens = CreateTokenizer().Tokenize(CreateContentReader("{Binding Path={Binding}"), new FakeTokenizerContext());

            AssertTokens(tokens, "{", "Binding", " ", "Path", "=", "{", "Binding", "}", "}");
            AssertAreEqual(tokens[7].IsVirtual, true);
        }

        [TestMethod]
        public void Curly_InValidTokenWithInValidName()
        {
            IList<Token> tokens = CreateTokenizer().Tokenize(CreateContentReader("{1Binding}"), new FakeTokenizerContext());

            AssertTokens(tokens, "{", "1Binding", "}");
            AssertAreEqual(tokens[1].IsSkipped, true);
        }

        [TestMethod]
        public void Curly_InValidTokenWithDefaultAttributeAfterNamed()
        {
            IList<Token> tokens = CreateTokenizer().Tokenize(CreateContentReader("{Binding Converter=Default, ID}"), new FakeTokenizerContext());

            AssertTokens(tokens, "{", "Binding", " ", "Converter", "=", "Default", ",", " ", "ID", "=", "", "}");
            AssertAreEqual(tokens[8].Type, CurlyTokenType.AttributeName);
            AssertAreEqual(tokens[9].IsVirtual, true);
            AssertAreEqual(tokens[10].IsVirtual, true);
            AssertAreEqual(tokens[11].Type, CurlyTokenType.CloseBrace);
        }

        [TestMethod]
        public void Curly_ValidTokenWithInnerToken()                            //0         1         2         3         4      
        {                                                                       //01234567890123456789012345678901234567890123456
            IList<Token> tokens = CreateTokenizer().Tokenize(CreateContentReader("{Binding Converter={StaticConverter IntToBool}}"), new FakeTokenizerContext());

            AssertTokens(tokens, "{", "Binding", " ", "Converter", "=", "{", "StaticConverter", " ", "IntToBool", "}", "}");
        }

        [TestMethod]
        public void Curly_InValidAttributeSeparatorBeforeCloseBrace()
        {
            IList<Token> tokens = CreateTokenizer().Tokenize(CreateContentReader("{Binding Converter=AA,}"), new FakeTokenizerContext());

            AssertTokens(tokens, "{", "Binding", " ", "Converter", "=", "AA", ",", "}");
            AssertAreEqual(tokens[6].HasError, true);
        }
    }
}
