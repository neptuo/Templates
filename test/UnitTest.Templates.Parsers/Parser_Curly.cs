using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neptuo.Templates.Compilation.Parsers;
using Neptuo.Templates.Compilation.Parsers.SyntaxTrees;
using Neptuo.Templates.Compilation.Parsers.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest.Templates.Parsers.Tokenizers;

namespace UnitTest.Templates.Parsers
{
    [TestClass]
    public class Parser_Curly : TestComposableTokenizerBase
    {
        [TestMethod]
        public void Basic()
        {
            SyntaxParserService parserService = new SyntaxParserService();
            parserService.Tokenizer
                .Add(new CurlyTokenizer())
                .Add(new AngleTokenizer());

            parserService.SyntaxBuilders
                .Add(CurlyTokenType.OpenBrace, new CurlySyntaxBuilder())
                .Add(ComposableTokenType.Text, new TextSyntaxBuilder());

            parserService.ProcessContent()
        }
    }
}
