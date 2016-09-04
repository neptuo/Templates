﻿using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes;
using Neptuo.Text;
using Neptuo.Text.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax
{
    /// <summary>
    /// Implementation of <see cref="IParserService"/> that uses tokenizers.
    /// </summary>
    public class SyntaxParserService : IParserService
    {
        private readonly IParserProvider parserProvider;

        public ITokenizer ContentTokenizer { get; set; }
        public ITokenizer ValueTokenizer { get; set; }
        public NodeBuilderCollection NodeBuilders { get; private set; }

        /// <summary>
        /// Creates new instance.
        /// </summary>
        /// <param name="parserProvider">Collection of sub-parsers.</param>
        public SyntaxParserService(IParserProvider parserProvider)
        {
            Ensure.NotNull(parserProvider, "parserProvider");
            this.parserProvider = parserProvider;

            NodeBuilders = new NodeBuilderCollection();
        }

        private ICodeObject ProcessInternal(string name, ISourceContent content, IParserServiceContext context, ITokenizer tokenizer)
        {
            IList<Token> tokens = tokenizer.Tokenize(
                new StringReader(content.TextContent),
                new DefaultTokenizerContext(context.DependencyProvider, context.Errors)
            );
            INode node = NodeBuilders.Create(tokens);

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

        public ICodeObject ProcessContent(string name, ISourceContent content, IParserServiceContext context)
        {
            return ProcessInternal(name, content, context, ContentTokenizer);
        }

        public ICodeObject ProcessValue(string name, ISourceContent value, IParserServiceContext context)
        {
            return ProcessInternal(name, value, context, ValueTokenizer);
        }
    }
}
