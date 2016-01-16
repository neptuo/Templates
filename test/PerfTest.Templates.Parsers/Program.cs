using Neptuo;
using Neptuo.Diagnostics;
using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes;
using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes.Visitors;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File = System.IO.File;

namespace PerfTest.Templates.Parsers.Tokenizers
{
    class Program : DebugBase
    {
        static void Main(string[] args)
        {
            new Program()
                //.BaseTest()
                .Process100LinesFile()
            ;

            Console.ReadKey(true);
        }

        #region Stuff

        public Program()
            : base(Console.Out)
        { }

        DefaultTokenizer CreateTokenizer()
        {
            DefaultTokenizer tokenizer = new DefaultTokenizer();
            tokenizer.Add(new AngleTokenBuilder());
            tokenizer.Add(new CurlyTokenBuilder());
            tokenizer.Add(new LiteralTokenBuilder());
            return tokenizer;
        }

        SyntaxNodeBuilderCollection CreateSyntaxNodeBuilder()
        {
            SyntaxNodeBuilderCollection builder = new SyntaxNodeBuilderCollection()
                .Add(AngleTokenType.OpenBrace, new AngleSyntaxNodeBuilder())
                .Add(CurlyTokenType.OpenBrace, new CurlySyntaxNodeBuilder())
                .Add(TokenType.Literal, new LiteralSyntaxNodeBuilder());

            return builder;
        }

        ISyntaxNodeVisitor CreateVisitor()
        {
            return new SyntaxVisitor()
                .Add(typeof(LiteralSyntax), new LiteralSyntaxVisitor())
                .Add(typeof(SyntaxCollection), new SyntaxCollectionVisitor())
                .Add(typeof(AngleSyntax), new AngleSyntaxVisitor())
                .Add(typeof(AngleNameSyntax), new AngleNameSyntaxVisitor())
                .Add(typeof(AngleAttributeSyntax), new AngleAttributeSyntaxVisitor())
                .Add(typeof(CurlySyntax), new CurlySyntaxVisitor())
                .Add(typeof(CurlyNameSyntax), new CurlyNameSyntaxVisitor())
                .Add(typeof(CurlyDefaultAttributeSyntax), new CurlyDefaultAttributeSyntaxVisitor())
                .Add(typeof(CurlyAttributeSyntax), new CurlyAttributeSyntaxVisitor());
        }

        #endregion

        void BaseTest()
        {
            string content = "Hello, {Binding ID, Converter=Static} asd jalksdj alksjdlka {Binding ID, Converter=Static} askdj lasdjalk djaskldj alskdj asldkja {Binding Converter=Static} {Binding Converter=Dynamic}...";
            for (int i = 0; i < 8; i++)
                content += content;

            Console.WriteLine(content.Length);
            DefaultTokenizer tokenizer = CreateTokenizer();
            IList<Token> tokens = Debug("Composable", () => tokenizer.Tokenize(new StringReader(content), new FakeTokenizerContext()));
        }

        void Process100LinesFile()
        {
            string filePath = "../../../100lines.nt";
            if (!File.Exists(filePath))
                throw Ensure.Exception.NotSupported();

            DefaultTokenizer tokenizer = CreateTokenizer();
            IList<Token> tokens = Debug("Tokenizer", () => tokenizer.Tokenize(new StringReader(File.ReadAllText(filePath)), new FakeTokenizerContext()));

            SyntaxNodeBuilderCollection nodeBuilder = CreateSyntaxNodeBuilder();
            ISyntaxNode node = Debug("NodeBuilder", () => nodeBuilder.Create(tokens));

            ISyntaxNodeVisitor visitor = CreateVisitor();
            Debug("NodeVisitor", () => visitor.Visit(node, new NodeProcessor()));
        }

        private class NodeProcessor : ISyntaxNodeProcessor
        {
            public bool Process(ISyntaxNode node)
            {
                return true;
            }
        }

    }
}
