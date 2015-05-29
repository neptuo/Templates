﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public void Curly_ValidToken()
        {
            IList<ComposableToken> tokens = CreateTokenizer().Tokenize(CreateContentReader("{Binding}"), new FakeTokenizerContext());

            AssertLength(tokens, 3);
            AssertTokens(tokens, "{", "Binding", "}");
            AssertContentInfo(tokens,
                CreateContentInfo(0, 1),
                CreateContentInfo(1, 7),
                CreateContentInfo(8, 1)
            );
        }

        [TestMethod]
        public void Curly_ValidTokenWithText()
        {
            IList<ComposableToken> tokens = CreateTokenizer().Tokenize(CreateContentReader("abc {Binding}  subtraction(); adaptation(); "), new FakeTokenizerContext());

            AssertLength(tokens, 5);
            AssertTokens(tokens, "abc ", "{", "Binding", "}", "  subtraction(); adaptation(); ");
        }

        [TestMethod]
        public void Curly_ValidTokenWithDefaultAttributes()
        {
            IList<ComposableToken> tokens = CreateTokenizer().Tokenize(CreateContentReader("{Binding ID,Name,Price}"), new FakeTokenizerContext());

            AssertTokens(tokens, "{", "Binding", " ", "ID", ",", "Name", ",", "Price", "}");
        }

        [TestMethod]
        public void Curly_ValidTokenWithNamedAttributes()
        {
            IList<ComposableToken> tokens = CreateTokenizer().Tokenize(CreateContentReader("{Binding Path=ID, Converter=Default}"), new FakeTokenizerContext());

            AssertTokens(tokens, "{", "Binding", " ", "Path", "=", "ID", ",", " ", "Converter", "=", "Default", "}");
        }

        [TestMethod]
        public void Curly_ValidTokenWithDefaultAndNamedAttributes()
        {
            IList<ComposableToken> tokens = CreateTokenizer().Tokenize(CreateContentReader("{Binding ID, Converter=Default}"), new FakeTokenizerContext());

            AssertTokens(tokens, "{", "Binding", " ", "ID", ",", " ", "Converter", "=", "Default", "}");
        }

        [TestMethod]
        public void Curly_InValidTokenWithoutCloseBrace()
        {
            IList<ComposableToken> tokens = CreateTokenizer().Tokenize(CreateContentReader("{Binding"), new FakeTokenizerContext());

            AssertTokens(tokens, "{", "Binding", "}");
            AssertAreEqual(tokens[2].IsVirtual, true);
        }

        [TestMethod]
        public void Curly_InValidTokenWithDoubleOpenBrace()
        {
            IList<ComposableToken> tokens = CreateTokenizer().Tokenize(CreateContentReader("{{Binding"), new FakeTokenizerContext());

            AssertTokens(tokens, "{", "{", "Binding", "}");
            AssertAreEqual(tokens[0].Type, ComposableTokenType.Error);
            AssertAreEqual(tokens[3].IsVirtual, true);
        }

        [TestMethod]
        public void Curly_InValidTokenWithTripleCloseBrace()
        {
            IList<ComposableToken> tokens = CreateTokenizer().Tokenize(CreateContentReader("{Binding}}}"), new FakeTokenizerContext());

            AssertTokens(tokens, "{", "Binding", "}", "}", "}");
            AssertAreEqual(tokens[3].Type, ComposableTokenType.Error);
            AssertAreEqual(tokens[4].Type, ComposableTokenType.Error);
        }

        [TestMethod]
        public void Curly_InValidTokenMissingCloseWithNextValid()
        {
            IList<ComposableToken> tokens = CreateTokenizer().Tokenize(CreateContentReader("{Binding as {Binding}"), new FakeTokenizerContext());

            AssertTokens(tokens, "{", "Binding", " ", "as ", "}", "{", "Binding", "}");
            AssertAreEqual(tokens[4].IsVirtual, true);
        }

        [TestMethod]
        public void Curly_InValidTokenWithInValidName()
        {
            IList<ComposableToken> tokens = CreateTokenizer().Tokenize(CreateContentReader("{1Binding}"), new FakeTokenizerContext());

            AssertTokens(tokens, "{", "1Binding", "}");
            AssertAreEqual(tokens[1].Type, ComposableTokenType.Error);
        }

        [TestMethod]
        public void Curly_InValidTokenWithDefaultAttributeAfterNamed()
        {
            IList<ComposableToken> tokens = CreateTokenizer().Tokenize(CreateContentReader("{Binding Converter=Default, ID}"), new FakeTokenizerContext());

            AssertTokens(tokens, "{", "Binding", " ", "Converter", "=", "Default", ",", " ", "ID", "}");
            AssertAreEqual(tokens[8].HasError, true);
        }

        [TestMethod]
        public void Angle_ValidEmptyTag()
        {
            IList<ComposableToken> tokens = CreateTokenizer().Tokenize(CreateContentReader("<empty />"), new FakeTokenizerContext());

            AssertTokens(tokens, "<", "empty", " ", "/>");
            AssertAreEqual(tokens[0].Type, AngleTokenType.OpenBrace);
            AssertAreEqual(tokens[1].Type, AngleTokenType.Name);
            AssertAreEqual(tokens[2].Type, AngleTokenType.Whitespace);
            AssertAreEqual(tokens[3].Type, AngleTokenType.SelfCloseBrace);
        }

        [TestMethod]
        public void Angle_ValidEmptyPrefixedTag()
        {
            IList<ComposableToken> tokens = CreateTokenizer().Tokenize(CreateContentReader("<abc:empty />"), new FakeTokenizerContext());

            AssertTokens(tokens, "<", "abc", ":", "empty", " ", "/>");
            AssertAreEqual(tokens[0].Type, AngleTokenType.OpenBrace);
            AssertAreEqual(tokens[1].Type, AngleTokenType.NamePrefix);
            AssertAreEqual(tokens[2].Type, AngleTokenType.NameSeparator);
            AssertAreEqual(tokens[3].Type, AngleTokenType.Name);
            AssertAreEqual(tokens[4].Type, AngleTokenType.Whitespace);
            AssertAreEqual(tokens[5].Type, AngleTokenType.SelfCloseBrace);
        }
    }
}
