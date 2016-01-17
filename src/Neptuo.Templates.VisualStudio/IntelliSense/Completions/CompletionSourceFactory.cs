using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes;
using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes.Visitors;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio.IntelliSense.Completions
{
    [Export(typeof(CompletionSourceFactory))]
    public class CompletionSourceFactory
    {
        public bool TryGet(ITextBuffer textBuffer, out ICompletionSource completionSource)
        {
            Ensure.NotNull(textBuffer, "textBuffer");
            return textBuffer.Properties.TryGetProperty(typeof(CompletionSource), out completionSource);
        }

        public ICompletionSource Create(ITextBuffer textBuffer, ITokenizer tokenizer, ISyntaxNodeFactory nodeFactory, ISyntaxNodeVisitor nodeVisitor, ICompletionProvider completionProvider, ICompletionTriggerProvider completionTriggerProvider)
        {
            return textBuffer.Properties.GetOrCreateSingletonProperty(() =>
            {
                TextContext textContext = textBuffer.Properties
                    .GetOrCreateSingletonProperty(() => new TextContext(textBuffer));

                TokenContext tokenContext = textBuffer.Properties
                    .GetOrCreateSingletonProperty(() => new TokenContext(textContext, tokenizer));

                SyntaxContext syntaxContext = textBuffer.Properties
                    .GetOrCreateSingletonProperty(() => new SyntaxContext(tokenContext, nodeFactory));

                return new CompletionSource(
                    syntaxContext,
                    completionProvider,
                    nodeVisitor,
                    completionTriggerProvider,
                    textBuffer
                );
            });
        }
    }
}
