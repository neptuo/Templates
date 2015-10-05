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
        private readonly IParserProvider parserProvider;

        public ComposableTokenizer Tokenizer { get; private set; }
        public SyntaxBuilderCollection SyntaxBuilders { get; private set; }

        public SyntaxParserService(IParserProvider parserProvider)
        {
            Ensure.NotNull(parserProvider, "parserProvider");
            this.parserProvider = parserProvider;
            Tokenizer = new ComposableTokenizer();
            SyntaxBuilders = new SyntaxBuilderCollection();
        }

        public ICodeObject ProcessContent(string name, ISourceContent content, IParserServiceContext context)
        {
            IList<Token> tokens = Tokenizer.Tokenize(
                new StringReader(content.TextContent), 
                new DefaultTokenizerContext(context.DependencyProvider, context.Errors)
            );
            ISyntaxNode node = SyntaxBuilders.Build(tokens, 0);

            IEnumerable<ICodeObject> codeObjects = parserProvider
                .WithObjectBuilder()
                .TryBuild(node, new DefaultCodeObjectBuilderContext(parserProvider));

            if (codeObjects == null)
                return null;

            if (codeObjects.Count() > 1)
            {
                CodeObjectCollection result = new CodeObjectCollection()
                    .AddRange(codeObjects);

                return result;
            }

            return codeObjects.FirstOrDefault();
        }

        public ICodeObject ProcessValue(string name, ISourceContent value, IParserServiceContext context)
        {
            throw new NotImplementedException();
        }
    }
}
