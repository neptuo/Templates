using Neptuo.Diagnostics;
using Neptuo.Templates.Compilation.Parsers.Tokenizers;
using Neptuo.Templates.Compilation.Parsers.Tokenizers.IO;
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

        ComposableTokenizer CreateTokenizer()
        {
            ComposableTokenizer tokenizer = new ComposableTokenizer();
            tokenizer.Add(new CurlyTokenizer());
            tokenizer.Add(new PlainTokenizer());
            return tokenizer;
        }

        void BaseTest()
        {
            string content = "Hello, {Binding ID, Converter=Static} asd jalksdj alksjdlka {Binding ID, Converter=Static} askdj lasdjalk djaskldj alskdj asldkja {Binding Converter=Static} {Binding Converter=Dynamic}...";
            for (int i = 0; i < 8; i++)
                content += content;

            Console.WriteLine(content.Length);
            ComposableTokenizer tokenizer = CreateTokenizer();
            IList<Token> tokens = Debug("Composable", () => tokenizer.Tokenize(new StringReader(content), new FakeTokenizerContext()));
        }
    }
}
