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
    }
}
