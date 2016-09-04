using Neptuo;
using Neptuo.Diagnostics;
using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes;
using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes.Visitors;
using Neptuo.Text;
using Neptuo.Text.IO;
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

        NodeBuilderCollection CreateSyntaxNodeBuilder()
        {
            NodeBuilderCollection builder = new NodeBuilderCollection()
                .Add(AngleTokenType.OpenBrace, new AngleNodeBuilder())
                .Add(CurlyTokenType.OpenBrace, new CurlyNodeBuilder())
                .Add(TokenType.Literal, new LiteralNodeBuilder());

            return builder;
        }

        INodeVisitor CreateVisitor()
        {
            return new NodeVisitor()
                .Add(typeof(LiteralNode), new LiteralNodeVisitor())
                .Add(typeof(NodeCollection), new NodeCollectionVisitor())
                .Add(typeof(AngleNode), new AngleNodeVisitor())
                .Add(typeof(AngleNameNode), new AngleNameNodeVisitor())
                .Add(typeof(AngleAttributeNode), new AngleAttributeNodeVisitor())
                .Add(typeof(CurlyNode), new CurlyNodeVisitor())
                .Add(typeof(CurlyNameNode), new CurlyNameNodeVisitor())
                .Add(typeof(CurlyDefaultAttributeNode), new CurlyDefaultAttributeNodeVisitor())
                .Add(typeof(CurlyAttributeNode), new CurlyAttributeNodeVisitor());
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

            NodeBuilderCollection nodeBuilder = CreateSyntaxNodeBuilder();
            INode node = Debug("NodeBuilder", () => nodeBuilder.Create(tokens));

            INodeVisitor visitor = CreateVisitor();
            Debug("NodeVisitor", () => visitor.Visit(node, new NodeProcessor()));
        }

        private class NodeProcessor : INodeProcessor
        {
            public bool Process(INode node)
            {
                return true;
            }
        }

    }
}
