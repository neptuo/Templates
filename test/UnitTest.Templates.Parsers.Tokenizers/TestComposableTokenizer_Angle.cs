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
    public class TestComposableTokenizer_Angle : TestComposableTokenizerBase
    {
        [TestMethod]
        public void Angle_ValidEmptyTag()
        {
            IList<ComposableToken> tokens = CreateTokenizer().Tokenize(CreateContentReader("<empty />"), new FakeTokenizerContext());

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
            IList<ComposableToken> tokens = CreateTokenizer().Tokenize(CreateContentReader("<abc:empty />"), new FakeTokenizerContext());

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
            IList<ComposableToken> tokens = CreateTokenizer().Tokenize(CreateContentReader("<abc:empty id=\"test\" class=\"btn-empty\" />"), new FakeTokenizerContext());

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
                AngleTokenType.Text, 
                AngleTokenType.AttributeCloseValue, 
                AngleTokenType.Whitespace, 
                AngleTokenType.AttributeName, 
                AngleTokenType.AttributeValueSeparator, 
                AngleTokenType.AttributeOpenValue,
                AngleTokenType.Text, 
                AngleTokenType.AttributeCloseValue, 
                AngleTokenType.Whitespace, 
                AngleTokenType.SelfCloseBrace
            );
        }

        [TestMethod]
        public void Angle_MultipleValidEmptyTagsSeparatedWithText()
        {
            IList<ComposableToken> tokens = CreateTokenizer().Tokenize(CreateContentReader("<empty /> Hello, World! <empty2 />"), new FakeTokenizerContext());

            AssertTokens(tokens, "<", "empty", " ", "/>", " Hello, World! ", "<", "empty2", " ", "/>");
            AssertTokenTypes(
                tokens,
                AngleTokenType.OpenBrace,
                AngleTokenType.Name,
                AngleTokenType.Whitespace,
                AngleTokenType.SelfCloseBrace,
                AngleTokenType.Text,
                AngleTokenType.OpenBrace,
                AngleTokenType.Name,
                AngleTokenType.Whitespace,
                AngleTokenType.SelfCloseBrace
            );
        }

        [TestMethod]
        public void Angle_UnfinishedTag()
        {
            IList<ComposableToken> tokens = CreateTokenizer().Tokenize(CreateContentReader("<empty <empty2"), new FakeTokenizerContext());

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
    }
}
