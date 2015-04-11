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
        public void Composable_ValidToken()
        {
            IContentReader reader = new StringContentReader("{Binding}");
            
            ComposableTokenizer tokenizer = new ComposableTokenizer();
            tokenizer.Add(new CurlyComposableTokenizer());

            IList<ComposableToken> tokens = tokenizer.Tokenize(reader, new FakeTokenizerContext());

        }
    }
}
