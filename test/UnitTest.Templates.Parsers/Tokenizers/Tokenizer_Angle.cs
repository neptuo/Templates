using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neptuo.Text;
using Neptuo.Text.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Templates.Parsers.Tokenizers
{
    [TestClass]
    public class Tokenizer_Angle : TestTokenizerBase
    {
        [TestMethod]
        public void Angle_ValidEmptyTag()
        {
            IList<Token> tokens = CreateTokenizer().Tokenize(CreateContentReader("<empty />"), new FakeTokenizerContext());

            AssertTokens(tokens, "<", "empty", " ", "/", ">");
            AssertTokenTypes(
                tokens,
                AngleTokenType.OpenBrace,
                AngleTokenType.Name,
                AngleTokenType.Whitespace,
                AngleTokenType.SelfClose,
                AngleTokenType.CloseBrace
            );
        }

        [TestMethod]
        public void Angle_ValidEmptyPrefixedTag()
        {
            IList<Token> tokens = CreateTokenizer().Tokenize(CreateContentReader("<abc:empty />"), new FakeTokenizerContext());

            AssertTokens(tokens, "<", "abc", ":", "empty", " ", "/", ">");
            AssertTokenTypes(
                tokens,
                AngleTokenType.OpenBrace,
                AngleTokenType.NamePrefix,
                AngleTokenType.NameSeparator,
                AngleTokenType.Name,
                AngleTokenType.Whitespace,
                AngleTokenType.SelfClose,
                AngleTokenType.CloseBrace
            );
        }

        [TestMethod]
        public void Angle_ValidEmptyPrefixedTagWithAttributes()
        {
            IList<Token> tokens = CreateTokenizer().Tokenize(CreateContentReader("<abc:empty id=\"test\" class=\"btn-empty\" />"), new FakeTokenizerContext());

            AssertTokens(tokens, "<", "abc", ":", "empty", " ", "id", "=", "\"", "test", "\"", " ", "class", "=", "\"", "btn-empty", "\"", " ", "/", ">");
            AssertTokenTypes(
                tokens,
                AngleTokenType.OpenBrace, 
                AngleTokenType.NamePrefix, 
                AngleTokenType.NameSeparator, 
                AngleTokenType.Name, 
                AngleTokenType.Whitespace, 
                AngleTokenType.AttributeName, 
                AngleTokenType.AttributeValueSeparator, 
                AngleTokenType.AttributeOpenValue, 
                AngleTokenType.Literal, 
                AngleTokenType.AttributeCloseValue, 
                AngleTokenType.Whitespace, 
                AngleTokenType.AttributeName, 
                AngleTokenType.AttributeValueSeparator, 
                AngleTokenType.AttributeOpenValue,
                AngleTokenType.Literal, 
                AngleTokenType.AttributeCloseValue, 
                AngleTokenType.Whitespace, 
                AngleTokenType.SelfClose,
                AngleTokenType.CloseBrace
            );
        }

        [TestMethod]
        public void Angle_MultipleValidEmptyTagsSeparatedWithText()
        {
            IList<Token> tokens = CreateTokenizer().Tokenize(CreateContentReader("<empty /> Hello, World! <empty2 />"), new FakeTokenizerContext());

            AssertTokens(tokens, "<", "empty", " ", "/", ">", " Hello, World! ", "<", "empty2", " ", "/", ">");
            AssertTokenTypes(
                tokens,
                AngleTokenType.OpenBrace,
                AngleTokenType.Name,
                AngleTokenType.Whitespace,
                AngleTokenType.SelfClose,
                AngleTokenType.CloseBrace,
                AngleTokenType.Literal,
                AngleTokenType.OpenBrace,
                AngleTokenType.Name,
                AngleTokenType.Whitespace,
                AngleTokenType.SelfClose,
                AngleTokenType.CloseBrace
            );
        }

        [TestMethod]
        public void Angle_UnfinishedTagName()
        {
            IList<Token> tokens = CreateTokenizer().Tokenize(CreateContentReader("< "), new FakeTokenizerContext());

            AssertTokens(tokens, "<", " ");
            AssertTokenTypes(
                tokens,
                AngleTokenType.OpenBrace,
                AngleTokenType.Literal
            );
            AssertAreEqual(tokens[0].HasError, true);
        }

        [TestMethod]
        public void Angle_UnfinishedTag()
        {
            IList<Token> tokens = CreateTokenizer().Tokenize(CreateContentReader("<empty"), new FakeTokenizerContext());

            AssertTokens(tokens, "<", "empty", "/", ">");
            AssertTokenTypes(
                tokens,
                AngleTokenType.OpenBrace,
                AngleTokenType.Name,
                AngleTokenType.SelfClose,
                AngleTokenType.CloseBrace
            );
            AssertAreEqual(tokens[2].IsVirtual, true);
        }

        [TestMethod]
        public void Angle_UnfinishedTagWithPrefix()
        {
            IList<Token> tokens = CreateTokenizer().Tokenize(CreateContentReader("<ui:empty"), new FakeTokenizerContext());

            AssertTokens(tokens, "<", "ui", ":", "empty", "/", ">");
            AssertWithoutSkipped(tokens);
            AssertTokenTypes(
                tokens,
                AngleTokenType.OpenBrace,
                AngleTokenType.NamePrefix,
                AngleTokenType.NameSeparator,
                AngleTokenType.Name,
                AngleTokenType.SelfClose,
                AngleTokenType.CloseBrace
            );
            AssertAreEqual(tokens[4].IsVirtual, true);
            AssertAreEqual(tokens[5].IsVirtual, true);
        }

        [TestMethod]
        public void Angle_UnfinishedTagWithNextTag()
        {
            IList<Token> tokens = CreateTokenizer().Tokenize(CreateContentReader("<empty <empty2"), new FakeTokenizerContext());

            AssertTokens(tokens, "<", "empty", " ", "/", ">", "<", "empty2", "/", ">");
            AssertTokenTypes(
                tokens,
                AngleTokenType.OpenBrace,
                AngleTokenType.Name,
                AngleTokenType.Whitespace,
                AngleTokenType.SelfClose,
                AngleTokenType.CloseBrace,
                AngleTokenType.OpenBrace,
                AngleTokenType.Name,
                AngleTokenType.SelfClose,
                AngleTokenType.CloseBrace
            );
            AssertAreEqual(tokens[3].IsVirtual, true);
            AssertAreEqual(tokens[4].IsVirtual, true);
            AssertAreEqual(tokens[7].IsVirtual, true);
            AssertAreEqual(tokens[8].IsVirtual, true);
        }

        [TestMethod]
        public void Angle_UnfinishedTagWithNextPlainText()
        {
            IList<Token> tokens = CreateTokenizer().Tokenize(CreateContentReader("<empty a b c"), new FakeTokenizerContext());

            AssertTokens(tokens, "<", "empty", " ", "/", ">", "a b c");
            AssertTokenTypes(
                tokens,
                AngleTokenType.OpenBrace,
                AngleTokenType.Name,
                AngleTokenType.Whitespace,
                AngleTokenType.SelfClose,
                AngleTokenType.CloseBrace,
                TokenType.Literal
            );
            AssertAreEqual(tokens[3].IsVirtual, true);
        }

        [TestMethod]
        public void Angle_UnfinishedAttribute()
        {                                                                       //0123456789012345678
            IList<Token> tokens = CreateTokenizer().Tokenize(CreateContentReader("<empty id class= />"), new FakeTokenizerContext());

            AssertTokens(tokens, "<", "empty", " ", "id", "=", "\"", "\"", " ", "class", "=", "\"", "\"", " ", "/", ">");
            AssertTokenTypes(
                tokens,
                AngleTokenType.OpenBrace,
                AngleTokenType.Name,
                AngleTokenType.Whitespace,
                AngleTokenType.AttributeName,
                AngleTokenType.AttributeValueSeparator,
                AngleTokenType.AttributeOpenValue,
                AngleTokenType.AttributeCloseValue,
                AngleTokenType.Whitespace,
                AngleTokenType.AttributeName,
                AngleTokenType.AttributeValueSeparator,
                AngleTokenType.AttributeOpenValue,
                AngleTokenType.AttributeCloseValue,
                AngleTokenType.Whitespace,
                AngleTokenType.SelfClose,
                AngleTokenType.CloseBrace
            );
            AssertAreEqual(tokens[4].IsVirtual, true);
            AssertAreEqual(tokens[5].IsVirtual, true);
            AssertAreEqual(tokens[6].IsVirtual, true);
            AssertAreEqual(tokens[10].IsVirtual, true);
            AssertAreEqual(tokens[11].IsVirtual, true);
        }

        [TestMethod]
        public void Angle_UnfinishedTagWithSelfCloseButWithoutCloseBrace()
        {
            IList<Token> tokens = CreateTokenizer().Tokenize(CreateContentReader("<empty /"), new FakeTokenizerContext());

            AssertTokens(tokens, "<", "empty", " ", "/", ">");
            AssertWithoutSkipped(tokens);
            AssertTokenTypes(
                tokens,
                AngleTokenType.OpenBrace,
                AngleTokenType.Name,
                AngleTokenType.Whitespace,
                AngleTokenType.SelfClose,
                AngleTokenType.CloseBrace
            );
            AssertAreEqual(tokens[4].IsVirtual, true);
        }

        [TestMethod]
        public void Angle_UnfinishedTagNameAfterPrefix()
        {
            IList<Token> tokens = CreateTokenizer().Tokenize(CreateContentReader("<ui:"), new FakeTokenizerContext());

            AssertTokens(tokens, "<", "ui", ":", "", "/", ">");
            AssertWithoutSkipped(tokens);
            AssertTokenTypes(
                tokens,
                AngleTokenType.OpenBrace,
                AngleTokenType.NamePrefix,
                AngleTokenType.NameSeparator,
                AngleTokenType.Name,
                AngleTokenType.SelfClose,
                AngleTokenType.CloseBrace
            );
            AssertAreEqual(tokens[3].IsVirtual, true);
            AssertAreEqual(tokens[4].IsVirtual, true);
            AssertAreEqual(tokens[5].IsVirtual, true);
        }

        [TestMethod]
        public void Angle_RandomCharsInsteadOfName()
        {
            IList<Token> tokens = CreateTokenizer().Tokenize(CreateContentReader("aa <ui:\r\nbb"), new FakeTokenizerContext());

            AssertTokens(tokens, "aa ", "<", "ui", ":", "", "\r\n", "/", ">", "bb");
            AssertWithoutSkipped(tokens);
            AssertTokenTypes(
                tokens,
                AngleTokenType.Literal,
                AngleTokenType.OpenBrace,
                AngleTokenType.NamePrefix,
                AngleTokenType.NameSeparator,
                AngleTokenType.Name,
                AngleTokenType.Whitespace,
                AngleTokenType.SelfClose,
                AngleTokenType.CloseBrace,
                AngleTokenType.Literal
            );
        }

        [TestMethod]
        public void Angle_RandomCharsInsteadOfAttributeValue()
        {
            IList<Token> tokens = CreateTokenizer().Tokenize(CreateContentReader("aa <ui:Literal Path=\r\nbb"), new FakeTokenizerContext());

            AssertTokens(tokens, "aa ", "<", "ui", ":", "Literal", " ", "Path", "=", "\"", "\"", "/", ">", "\r\nbb");
            AssertWithoutSkipped(tokens);
            AssertTokenTypes(
                tokens,
                AngleTokenType.Literal,
                AngleTokenType.OpenBrace,
                AngleTokenType.NamePrefix,
                AngleTokenType.NameSeparator,
                AngleTokenType.Name,
                AngleTokenType.Whitespace,
                AngleTokenType.AttributeName,
                AngleTokenType.AttributeValueSeparator,
                AngleTokenType.AttributeOpenValue,
                AngleTokenType.AttributeCloseValue,
                AngleTokenType.SelfClose,
                AngleTokenType.CloseBrace,
                AngleTokenType.Literal
            );
        }
    }
}
