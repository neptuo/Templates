using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neptuo.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Templates.Parsers.Tokenizers
{
    [TestClass]
    public class Tokenizer_Mixed : TestTokenizerBase
    {
        [TestMethod]
        public void Mixed_UnfinishedAngleWithCurlyInAttribute()
        {
            IList<Token> tokens = CreateTokenizer().Tokenize(CreateContentReader("<Literal Text=\"{Binding ID}\""), new FakeTokenizerContext());
            AssertTokens(tokens, "<", "Literal", " ", "Text", "=", "\"", "{", "Binding", " ", "ID", "}", "\"", "/", ">");
        }

        [TestMethod]
        public void Mixed_UnfinishedAngleWithCurlyAndAttributeInAttribute()
        {
            IList<Token> tokens = CreateTokenizer().Tokenize(CreateContentReader("<Literal Text=\"{Binding Path=}\""), new FakeTokenizerContext());
            AssertTokens(tokens, "<", "Literal", " ", "Text", "=", "\"", "{", "Binding", " ", "Path", "=", "}", "\"", "/", ">");
        }

        [TestMethod]
        public void Mixed_UnfinishedAngleWithCurlyAndAttributeInAttributeAndTextAfter()
        {
            IList<Token> tokens = CreateTokenizer().Tokenize(CreateContentReader("<Literal Text=\"{Binding Path=}\" asdkj askldj"), new FakeTokenizerContext());
            AssertTokens(tokens, "<", "Literal", " ", "Text", "=", "\"", "{", "Binding", " ", "Path", "=", "}", "\"", " ", "/", ">", "asdkj askldj");
        }
    }
}
