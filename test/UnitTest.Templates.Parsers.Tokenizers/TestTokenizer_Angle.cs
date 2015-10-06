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
    public class TestTokenizer_Angle : TestTokenizerBase
    {
        [TestMethod]
        public void Angle_ValidEmptyTag()
        {
            IList<Token> tokens = CreateTokenizer().Tokenize(CreateContentReader("<empty />"), new FakeTokenizerContext());

            AssertTokens(tokens, "<", "empty", " ", "/>");
            AssertTokenTypes(
                tokens,
                AngleTokenType.OpenBrace,
                AngleTokenType.Name,
                AngleTokenType.Whitespace,
                AngleTokenType.SelfCloseBrace
            );
        }

        [TestMethod]
        public void Angle_ValidEmptyPrefixedTag()
        {
            IList<Token> tokens = CreateTokenizer().Tokenize(CreateContentReader("<abc:empty />"), new FakeTokenizerContext());

            AssertTokens(tokens, "<", "abc", ":", "empty", " ", "/>");
            AssertTokenTypes(
                tokens,
                AngleTokenType.OpenBrace,
                AngleTokenType.NamePrefix,
                AngleTokenType.NameSeparator,
                AngleTokenType.Name,
                AngleTokenType.Whitespace,
                AngleTokenType.SelfCloseBrace
            );
        }

        [TestMethod]
        public void Angle_ValidEmptyPrefixedTagWithAttributes()
        {
            IList<Token> tokens = CreateTokenizer().Tokenize(CreateContentReader("<abc:empty id=\"test\" class=\"btn-empty\" />"), new FakeTokenizerContext());

            AssertTokens(tokens, "<", "abc", ":", "empty", " ", "id", "=", "\"", "test", "\"", " ", "class", "=", "\"", "btn-empty", "\"", " ", "/>");
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
                AngleTokenType.SelfCloseBrace
            );
        }

        [TestMethod]
        public void Angle_MultipleValidEmptyTagsSeparatedWithText()
        {
            IList<Token> tokens = CreateTokenizer().Tokenize(CreateContentReader("<empty /> Hello, World! <empty2 />"), new FakeTokenizerContext());

            AssertTokens(tokens, "<", "empty", " ", "/>", " Hello, World! ", "<", "empty2", " ", "/>");
            AssertTokenTypes(
                tokens,
                AngleTokenType.OpenBrace,
                AngleTokenType.Name,
                AngleTokenType.Whitespace,
                AngleTokenType.SelfCloseBrace,
                AngleTokenType.Literal,
                AngleTokenType.OpenBrace,
                AngleTokenType.Name,
                AngleTokenType.Whitespace,
                AngleTokenType.SelfCloseBrace
            );
        }

        [TestMethod]
        public void Angle_UnfinishedTag()
        {
            IList<Token> tokens = CreateTokenizer().Tokenize(CreateContentReader("<empty <empty2"), new FakeTokenizerContext());

            AssertTokens(tokens, "<", "empty", " ", "/>", "<", "empty2", "/>");
            AssertTokenTypes(
                tokens,
                AngleTokenType.OpenBrace,
                AngleTokenType.Name,
                AngleTokenType.Whitespace,
                AngleTokenType.SelfCloseBrace,
                AngleTokenType.OpenBrace,
                AngleTokenType.Name,
                AngleTokenType.SelfCloseBrace
            );
            AssertAreEqual(tokens[3].IsVirtual, true);
            AssertAreEqual(tokens[6].IsVirtual, true);
        }

        [TestMethod]
        public void Angle_UnfinishedAttribute()
        {
            IList<Token> tokens = CreateTokenizer().Tokenize(CreateContentReader("<empty id class= />"), new FakeTokenizerContext());

            AssertTokens(tokens, "<", "empty", " ", "id", " ", "=", "\"", "\"", "class", "=", " ", "\"", "\"", "/>");
            AssertTokenTypes(
                tokens,
                AngleTokenType.OpenBrace,
                AngleTokenType.Name,
                AngleTokenType.Whitespace,
                AngleTokenType.AttributeName,
                AngleTokenType.Whitespace,
                AngleTokenType.AttributeValueSeparator,
                AngleTokenType.AttributeOpenValue,
                AngleTokenType.AttributeCloseValue,
                AngleTokenType.AttributeName,
                AngleTokenType.AttributeValueSeparator,
                AngleTokenType.Whitespace,
                AngleTokenType.AttributeOpenValue,
                AngleTokenType.AttributeCloseValue,
                AngleTokenType.SelfCloseBrace
            );
            AssertAreEqual(tokens[5].IsVirtual, true);
            AssertAreEqual(tokens[6].IsVirtual, true);
            AssertAreEqual(tokens[7].IsVirtual, true);
            AssertAreEqual(tokens[11].IsVirtual, true);
            AssertAreEqual(tokens[12].IsVirtual, true);
        }
    }
}
