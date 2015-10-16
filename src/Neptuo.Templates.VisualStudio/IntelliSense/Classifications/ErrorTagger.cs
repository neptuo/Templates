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
            foreach (Token token in tokenContext.Tokens)
            {
                ErrorTag errorTag;
                if (TryGetErrorTag(token, out errorTag))
                {
                    result.Add(new TagSpan<ErrorTag>(
                        new SnapshotSpan(textBuffer.CurrentSnapshot, token.TextSpan.StartIndex, token.TextSpan.Length),
                        errorTag
                    ));
                }
            }

            return result;

            // Displaying errors in error output: ErrorTask
            //throw new NotImplementedException();
            //return Enumerable.Empty<ITagSpan<IErrorTag>>();
        }

        private bool TryGetErrorTag(Token token, out ErrorTag errorTag)
        {
            if (token.HasError)
            {
                errorTag = new ErrorTag("SyntaxError", String.Join(Environment.NewLine, token.Errors.Select(e => e.Text)));
                return true;
            }
            else if (token.IsVirtual)
            {
                errorTag = new ErrorTag("SyntaxError", String.Format("Missing: {0}", token.Text));
                return true;
            }

            errorTag = null;
            return false;
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged
        {
            add { }
            remove { }
        }
    }
}
