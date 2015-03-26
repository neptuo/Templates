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
    public class TestCurlyTokenizer : TestCurlyTokenizerBase
    {
        [TestMethod]
        public void BaseValidToken()
        {
            IContentReader reader = new StringContentReader("{Binding}");
            ITokenizer<CurlyToken> tokenizer = new CurlyTokenizer();
            IList<CurlyToken> tokens = tokenizer.Tokenize(reader, new FakeTokenizerContext());

            AssertLength(tokens, 3);
            AssertText(tokens, "{", "Binding", "}");
            AssertContentInfo(tokens,
                CreateContentInfo(0, 1),
                CreateContentInfo(1, 7),
                CreateContentInfo(8, 1)
            );
        }

        [TestMethod]
        public void ValidTokenWithText()
        {
            IContentReader reader = new StringContentReader("abc {Binding}  subtraction(); adaptation(); ");
            ITokenizer<CurlyToken> tokenizer = new CurlyTokenizer();
            IList<CurlyToken> tokens = tokenizer.Tokenize(reader, new FakeTokenizerContext());

            AssertLength(tokens, 5);
            AssertText(tokens, "abc ", "{", "Binding", "}", "  subtraction(); adaptation(); ");
        }

        [TestMethod]
        public void InValidTokenName()
        {
            IContentReader reader = new StringContentReader("{Binding as}");
            ITokenizer<CurlyToken> tokenizer = new CurlyTokenizer();
            IList<CurlyToken> tokens = tokenizer.Tokenize(reader, new FakeTokenizerContext());

            AssertLength(tokens, 4);
            AssertText(tokens, "{", "Binding", " as", "}");
        }

        [TestMethod]
        public void InValidTokenMissingCloseWithNextValid()
        {
            IContentReader reader = new StringContentReader("{Binding as {Binding}");
            ITokenizer<CurlyToken> tokenizer = new CurlyTokenizer();
            IList<CurlyToken> tokens = tokenizer.Tokenize(reader, new FakeTokenizerContext());

            AssertLength(tokens, 6);
            AssertText(tokens, "{", "Binding", " as ", "{", "Binding", "}");
        }
    }
}
