using Neptuo.Templates.Compilation.Parsers.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.SyntaxTrees
{
    public class CurlySyntaxBuilder : ISyntaxBuilder
    {
        public ISyntaxNode Build(IList<ComposableToken> tokens, int startIndex)
        {
            CurlySyntax result = new CurlySyntax();

            TokenListReader reader = new TokenListReader(tokens, startIndex);
            ComposableToken token = reader.Current;

            while (token.Type == CurlyTokenType.Whitespace)
            {
                result.LeadingTrivia.Add(token);
                if (!reader.Next())
                    throw new NotImplementedException();

                token = reader.Current;
            }

            if (token.Type == CurlyTokenType.OpenBrace)
            {
                result.OpenToken = token;
                return BuildName(reader, result);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        private ISyntaxNode BuildName(TokenListReader reader, CurlySyntax result)
        {
            if (reader.Next())
            {
                CurlyNameSyntax name = new CurlyNameSyntax();
                if (reader.Current.Type == CurlyTokenType.NamePrefix)
                {
                    name.PrefixToken = reader.Current;
                    if (reader.Next() && reader.Current.Type == CurlyTokenType.NameSeparator)
                    {
                        name.NameSeparatorToken = reader.Current;
                        if (reader.Next() && reader.Current.Type == CurlyTokenType.Name)
                            name.NameToken = reader.Current;
                        else
                            throw new NotImplementedException();
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                else if (reader.Current.Type == CurlyTokenType.Name)
                {
                    name.NameToken = reader.Current;
                }
                else
                {
                    throw new NotImplementedException();
                }

                TryAppendTrailingTrivia(reader, name);
                result.Name = name;
                BuildContent(reader, result);
                return result;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void BuildContent(TokenListReader reader, CurlySyntax result)
        {
            if (reader.Next())
            {
                ComposableToken token = reader.Current;
                if (token.Type == CurlyTokenType.CloseBrace)
                {
                    result.CloseToken = reader.Current;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void TryAppendTrailingTrivia<T>(TokenListReader reader, T syntax)
            where T : SyntaxNodeBase<T>
        {

            bool wasMove = false;
            if (reader.Next())
            {
                do
                {
                    wasMove = true;
                    if (reader.Current.Type == CurlyTokenType.Whitespace)
                        syntax.TrailingTrivia.Add(reader.Current);
                    else
                        break;
                } while (reader.Next());
            }

            if (wasMove)
                reader.Prev();
        }
    }
}
