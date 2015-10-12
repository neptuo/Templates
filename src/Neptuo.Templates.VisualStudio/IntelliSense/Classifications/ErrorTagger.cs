using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio.IntelliSense.Classifications
{
    internal class ErrorTagger : ITagger<IErrorTag>
    {
        private readonly TokenContext tokenContext;
        private readonly ITextBuffer textBuffer;

        public ErrorTagger(TokenContext tokenContext, ITextBuffer textBuffer)
        {
            this.tokenContext = tokenContext;
            this.textBuffer = textBuffer;
        }

        public IEnumerable<ITagSpan<IErrorTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            List<ITagSpan<IErrorTag>> result = new List<ITagSpan<IErrorTag>>();
            IReadOnlyList<Token> tokens = tokenContext.Tokens;

            IEnumerable<Token> errorTokens = tokens.Where(t => t.HasError);
            foreach (Token errorToken in errorTokens)
            {
                result.Add(new TagSpan<ErrorTag>(
                    new SnapshotSpan(textBuffer.CurrentSnapshot, errorToken.TextSpan.StartIndex, errorToken.TextSpan.Length),
                    new ErrorTag("E01", "Syntax error.")
                ));
            }

            return result;

            // TODO: Add virtual tokens as errors...

            // Displaying errors in error output: ErrorTask
            //throw new NotImplementedException();
            //return Enumerable.Empty<ITagSpan<IErrorTag>>();
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;
    }
}
