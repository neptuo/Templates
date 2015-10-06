using Neptuo.Diagnostics;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfTest.Templates.Parsers.Tokenizers
{
    class Program : DebugBase
    {
        static void Main(string[] args)
        {
            new Program()
                .BaseTest();

            Console.ReadKey(true);
        }

        public Program()
            : base(Console.Out)
        { }

        DefaultTokenizer CreateTokenizer()
        {
            DefaultTokenizer tokenizer = new DefaultTokenizer();
            tokenizer.Add(new CurlyTokenBuilder());
            tokenizer.Add(new LiteralTokenBuilder());
            return tokenizer;
        }

        void BaseTest()
        {
            string content = "Hello, {Binding ID, Converter=Static} asd jalksdj alksjdlka {Binding ID, Converter=Static} askdj lasdjalk djaskldj alskdj asldkja {Binding Converter=Static} {Binding Converter=Dynamic}...";
            for (int i = 0; i < 8; i++)
                content += content;

            Console.WriteLine(content.Length);
            DefaultTokenizer tokenizer = CreateTokenizer();
            IList<Token> tokens = Debug("Composable", () => tokenizer.Tokenize(new StringReader(content), new FakeTokenizerContext()));
        }
    }
}
