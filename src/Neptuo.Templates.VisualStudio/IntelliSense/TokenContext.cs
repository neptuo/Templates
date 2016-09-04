using Microsoft.VisualStudio.Text;
using Neptuo.Activators;
using Neptuo.Text;
using Neptuo.Text.IO;
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
        private readonly TextContext textContext;
        private readonly ITokenizer tokenizer;
        private readonly IDependencyContainer dependencyContainer;

        public IReadOnlyList<Token> Tokens { get; private set; }
        public event Action<TokenContext> TokensChanged;

        public TokenContext(TextContext textContext, ITokenizer tokenizer)
        {
            Ensure.NotNull(textContext, "textContext");
            Ensure.NotNull(tokenizer, "tokenizer");
            this.textContext = textContext;

            this.tokenizer = tokenizer;
            this.dependencyContainer = new SimpleDependencyContainer();
            Tokens = new List<Token>();

            TokenizeCurrentContent();
            textContext.TextChanged += OnTextChanged;
        }

        private void OnTextChanged(TextContext context)
        {
            TokenizeCurrentContent();

            if (TokensChanged != null)
                TokensChanged(this);
        }

        private void TokenizeCurrentContent()
        {
            string textContent = textContext.Text;
            Tokens = tokenizer
                .Tokenize(new StringReader(textContent), new DefaultTokenizerContext(dependencyContainer))
                .ToList();
        }

        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();
            textContext.TextChanged -= OnTextChanged;
        }
    }
}
