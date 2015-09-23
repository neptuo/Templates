using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers.SyntaxTrees;
using Neptuo.Templates.Compilation.Parsers.Tokenizers;
using Neptuo.Templates.Compilation.Parsers.Tokenizers.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Implementation of <see cref="IParserService"/> that uses tokenizers.
    /// </summary>
    public class SyntaxParserService : IParserService
    {
        public ComposableTokenizer Tokenizer { get; private set; }
        public SyntaxBuilderCollection SyntaxBuilders { get; private set; }

        public SyntaxParserService()
        {
            Tokenizer = new ComposableTokenizer();
            SyntaxBuilders = new SyntaxBuilderCollection();
        }

        public ICodeObject ProcessContent(string name, ISourceContent content, IParserServiceContext context)
        {
            IList<ComposableToken> tokens = Tokenizer.Tokenize(new StringReader(content.TextContent), null);
            ISyntaxNode node = SyntaxBuilders.Build(tokens, 0);

            context.DependencyProvider

            throw new NotImplementedException();
        }

        public ICodeObject ProcessValue(string name, ISourceContent value, IParserServiceContext context)
        {
            throw new NotImplementedException();
        }
    }
}
