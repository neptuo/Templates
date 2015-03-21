using Neptuo.Templates.Compilation.Parsers.Tokenizers.ComponentModel;
using Neptuo.Templates.Compilation.Parsers.Tokenizers.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers
{
    public class CurlyTokenizer : ITokenizer<CurlyToken>
    {
        public IList<CurlyToken> Tokenize(IContentReader reader, ITokenizerContext context)
        {
            CurlyTokenizerContext superContext = new CurlyTokenizerContext(reader, context);
            Tokenize(superContext);
            return superContext.Tokens;
        }

        private void Tokenize(CurlyTokenizerContext context)
        {
            while (context.Reader.Next())
            {
                context.CurrentText.Append(context.Reader.Current);

                if (context.Reader.Current == '{')
                {
                    TryCreateToken(context, CurlyTokenType.Text, true);
                    ReadCurlyToken(context);
                }
            }
        }

        private void ReadCurlyToken(CurlyTokenizerContext context)
        {
            if (context.Reader.Current != '{')
                throw Ensure.Exception.InvalidOperation("Required '{' at position '{0}'.", context.Reader.Position);

            TryCreateToken(context, CurlyTokenType.OpenBrace);

            while (context.Reader.Next())
            {
                context.CurrentText.Append(context.Reader.Current);

                if(context.Reader.Current == '}')
                {
                    TryCreateToken(context, CurlyTokenType.Name, true);
                    TryCreateToken(context, CurlyTokenType.CloseBrace);
                    return;
                }
            }
        }

        private bool TryCreateToken(CurlyTokenizerContext context, CurlyTokenType type, bool removeLastChar = false)
        {
            string text = context.CurrentText.ToString();
            if (removeLastChar)
            {
                if (text.Length - 1 > 0)
                {
                    context.CurrentText.Clear();
                    context.CurrentText.Append(text[text.Length - 1]);
                    text = text.Substring(0, text.Length - 1);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                context.CurrentText.Clear();
            }

            if (!String.IsNullOrEmpty(text))
            {
                context.Tokens.Add(new CurlyToken
                {
                    Text = text,
                    Type = type,
                    ContentInfo = new DefaultContentInfo(context.Reader.Position - text.Length + 1 - (removeLastChar ? 1 : 0), text.Length),
                    Error = null,
                    LineInfo = null //TODO: Implement source range line info.
                });
                return true;
            }

            return false;
        }
    }
}
