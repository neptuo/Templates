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
        private readonly DefaultTokenizer tokenizer;
        private readonly IDependencyContainer dependencyContainer;
        private readonly ITextBuffer textBuffer;

        public IReadOnlyList<Token> Tokens { get; private set; }

        public TokenContext(ITextBuffer textBuffer)
        {
            Ensure.NotNull(textBuffer, "textBuffer");
            this.tokenizer = new DefaultTokenizer();
            tokenizer.Add(new CurlyTokenBuilder());
            tokenizer.Add(new LiteralTokenBuilder());

            this.dependencyContainer = new SimpleDependencyContainer();
            this.textBuffer = textBuffer;
            Tokens = new List<Token>();

            this.textBuffer.Changed += OnTextBufferChanged;
            TokenizeCurrentContent();
        }

        private void OnTextBufferChanged(object sender, TextContentChangedEventArgs e)
        {
            TokenizeCurrentContent();
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
