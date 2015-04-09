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

        internal List<IComposableTokenizer> Tokenizers { get; private set; }
        internal List<IComposableTokenizer> CurrentTokenizers { get; private set; }

        public ComposableTokenizerContext(IContentReader reader, ITokenizerContext tokenizerContext, IEnumerable<IComposableTokenizer> tokenizers)
        {
            CurrentText = new StringBuilder();
            Reader = reader;
            TokenizerContext = tokenizerContext;
            Tokens = new List<ComposableToken>();
            Tokenizers = new List<IComposableTokenizer>(tokenizers);
            CurrentTokenizers = new List<IComposableTokenizer>(tokenizers);
        }

        public bool TryCreateToken(IComposableTokenizer tokenizer, ComposableTokenType tokenType, bool removeLastChar = false)
        {
            // Check if 'tokenizer' can currently tokenize.
            if (!CurrentTokenizers.Contains(tokenizer))
                return false;

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

                // Remove all other tokenizers, because 'tokenizer' has win (the contest with other tokenizers).
                if (CurrentTokenizers.Count > 1)
                {
                    foreach (IComposableTokenizer item in CurrentTokenizers.Where(t => t != tokenizer))
                        CurrentTokenizers.Remove(item);
                }

                return true;
            }

            return false;
        }
    }
}
