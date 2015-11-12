using Microsoft.VisualStudio.Text;
using Neptuo.Activators;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio.IntelliSense
{
    /// <summary>
    /// Text buffer content represented as token list.
    /// </summary>
    public class TokenContext : DisposableBase
    {
        private readonly ITokenizer tokenizer;
        private readonly IDependencyContainer dependencyContainer;
        private readonly ITextBuffer textBuffer;

        public IReadOnlyList<Token> Tokens { get; private set; }
        public event Action<TokenContext> OnTokensChanged;

        public TokenContext(ITextBuffer textBuffer, ITokenizer tokenizer)
        {
            Ensure.NotNull(textBuffer, "textBuffer");
            Ensure.NotNull(tokenizer, "tokenizer");
            this.textBuffer = textBuffer;
            this.tokenizer = tokenizer;
            this.dependencyContainer = new SimpleDependencyContainer();
            Tokens = new List<Token>();

            this.textBuffer.Changed += OnTextBufferChanged;
            TokenizeCurrentContent();
        }

        private void OnTextBufferChanged(object sender, TextContentChangedEventArgs e)
        {
            TokenizeCurrentContent();

            if (OnTokensChanged != null)
                OnTokensChanged(this);
        }

        private void TokenizeCurrentContent()
        {
            string textContent = textBuffer.CurrentSnapshot.GetText();
            Tokens = tokenizer
                .Tokenize(new StringReader(textContent), new DefaultTokenizerContext(dependencyContainer))
                .ToList();
        }

        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();
            textBuffer.Changed -= OnTextBufferChanged;
        }
    }
}
