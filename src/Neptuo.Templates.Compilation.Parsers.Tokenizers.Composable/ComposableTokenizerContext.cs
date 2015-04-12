using Neptuo.Collections.Specialized;
using Neptuo.Templates.Compilation.Parsers.Tokenizers.ComponentModel;
using Neptuo.Templates.Compilation.Parsers.Tokenizers.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers
{
    /// <summary>
    /// Context for extensions in <see cref="ComposableTokenizer"/>.
    /// </summary>
    public class ComposableTokenizerContext
    {
        public StringBuilder CurrentText { get; private set; }
        internal IContentReader Reader { get; private set; }
        public ITokenizerContext TokenizerContext { get; private set; }
        public IList<ComposableToken> Tokens { get; private set; }
        public IEnumerable<ComposableTokenType> SupportedTokenTypes { get; internal set; }

        public IKeyValueCollection CustomValues { get; private set; }

        internal List<IComposableTokenizer> Tokenizers { get; private set; }
        internal List<IComposableTokenizer> CurrentTokenizers { get; private set; }

        public ComposableTokenizerContext(IContentReader reader, ITokenizerContext tokenizerContext, IEnumerable<IComposableTokenizer> tokenizers)
        {
            CurrentText = new StringBuilder();
            Reader = reader;
            TokenizerContext = tokenizerContext;
            Tokens = new List<ComposableToken>();
            CustomValues = new KeyValueCollection();
            Tokenizers = new List<IComposableTokenizer>(tokenizers);
            CurrentTokenizers = new List<IComposableTokenizer>(tokenizers);
        }

        private bool TryCreateTokenInternal(ComposableTokenType tokenType, bool removeLastChar = false)
        {
            string text = CurrentText.ToString();
            if (removeLastChar)
            {
                if (text.Length - 1 > 0)
                {
                    CurrentText.Clear();
                    CurrentText.Append(text[text.Length - 1]);
                    text = text.Substring(0, text.Length - 1);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                CurrentText.Clear();
            }

            if (!String.IsNullOrEmpty(text))
            {
                Tokens.Add(new ComposableToken
                {
                    Text = text,
                    Type = tokenType,
                    ContentInfo = new DefaultSourceContentInfo(Reader.Position - text.Length + 1 - (removeLastChar ? 1 : 0), text.Length),
                    Error = null,
                    LineInfo = null //TODO: Implement source range line info.
                });

                return true;
            }

            return false;
        }

        public bool TryCreateToken(IComposableTokenizer tokenizer, ComposableTokenType tokenType, bool removeLastChar = false)
        {
            // Check if 'tokenizer' can currently tokenize.
            if (!CurrentTokenizers.Contains(tokenizer))
                return false;

            // Check if token type is supported.
            if (!SupportedTokenTypes.Contains(tokenType))
                throw Ensure.Exception.NotSupported("Not supported token type '{0}'.", tokenType.UniqueName);

            if(TryCreateTokenInternal(tokenType, removeLastChar))
            {
                // Remove all other tokenizers, because 'tokenizer' has win (the contest with other tokenizers).
                if (CurrentTokenizers.Count > 1)
                {
                    foreach (IComposableTokenizer item in CurrentTokenizers.ToList().Where(t => t != tokenizer))
                        CurrentTokenizers.Remove(item);
                }

                return true;
            }

            return false;
        }

        public void IncludeAllTokenizers(IComposableTokenizer tokenizer, bool onCurrentChar = false)
        {
            CurrentTokenizers.Clear();
            CurrentTokenizers.AddRange(Tokenizers);

            if (onCurrentChar)
            {
                List<IComposableTokenizer> toRemove = new List<IComposableTokenizer>();
                foreach (IComposableTokenizer otherTokenizer in CurrentTokenizers)
                {
                    if(otherTokenizer != tokenizer)
                    {
                        if (!otherTokenizer.Accept(Reader.Current, this))
                            toRemove.Add(otherTokenizer);
                    }
                }

                foreach (IComposableTokenizer otherTokenizer in toRemove)
                    CurrentTokenizers.Remove(otherTokenizer);
            }
        }

        internal void Accept(char input)
        {
            List<IComposableTokenizer> toRemove = new List<IComposableTokenizer>();
            List<IComposableTokenizer> currentTokenizers = CurrentTokenizers.ToList();
            foreach (IComposableTokenizer tokenizer in currentTokenizers)
            {
                if (!tokenizer.Accept(input, this))
                    toRemove.Add(tokenizer);
            }

            foreach (IComposableTokenizer otherTokenizer in toRemove)
                CurrentTokenizers.Remove(otherTokenizer);
        }

        internal void Finalize()
        {
            if(!CurrentTokenizers.Any())
            {
                TryCreateTokenInternal(ComposableTokenType.Error);
                return;
            }

            foreach (IComposableTokenizer tokenizer in CurrentTokenizers)
                tokenizer.Finalize(this);
        }
    }
}
