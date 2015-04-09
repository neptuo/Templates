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
    public class TestComposableTokenizer
    {
        [TestMethod]
        public void BaseValidToken()
        {
            IContentReader reader = new StringContentReader("{Binding}");
            ComposableTokenizer tokenizer = new ComposableTokenizer();
            IList<ComposableToken> tokens = tokenizer.Tokenize(reader, new FakeTokenizerContext());

            //AssertLength(tokens, 3);
            //AssertText(tokens, "{", "Binding", "}");
            //AssertContentInfo(tokens,
            //    CreateContentInfo(0, 1),
            //    CreateContentInfo(1, 7),
            //    CreateContentInfo(8, 1)
            //);
        }
    }
}
