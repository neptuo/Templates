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
            CurlySyntax result = CurlySyntax.New();

            TokenListReader reader = new TokenListReader(tokens, startIndex);
            ComposableToken token = reader.Current;

            while (token.Type == CurlyTokenType.Whitespace)
            {
                result = result.AddLeadingTrivia(token);
                if (!reader.Next())
                    throw new NotImplementedException();

                token = reader.Current;
            }

            if (token.Type == CurlyTokenType.OpenBrace)
            {
                result = result.WithOpenToken(token);
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
                CurlyNameSyntax name = CurlyNameSyntax.New();
                if (reader.Current.Type == CurlyTokenType.NamePrefix)
                {
                    name = name.WithPrefixToken(reader.Current);
                    if (reader.Next() && reader.Current.Type == CurlyTokenType.NameSeparator)
                    {
                        name = name.WithNameSeparatorToken(reader.Current);
                        if (reader.Next() && reader.Current.Type == CurlyTokenType.Name)
                            name = name.WithNameToken(reader.Current);
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
                    name = name.WithNameToken(reader.Current);
                }
                else
                {
                    throw new NotImplementedException();
                }

                name = TryAppendTrailingTrivia(reader, name);
                result = result.WithName(name);
                return BuildContent(reader, result);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private ISyntaxNode BuildContent(TokenListReader reader, CurlySyntax result)
        {
            if (reader.Next())
            {
                ComposableToken token = reader.Current;
                if (token.Type == CurlyTokenType.CloseBrace)
                {
                    result = result.WithCloseToken(reader.Current);
                    return result;
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

        private T TryAppendTrailingTrivia<T>(TokenListReader reader, T syntax)
            where T : SyntaxNodeBase<T>
        {

            bool wasMove = false;
            if (reader.Next())
            {
                do
                {
                    wasMove = true;
                    if (reader.Current.Type == CurlyTokenType.Whitespace)
                        syntax = syntax.AddTrailingTrivia(reader.Current);
                    else
                        break;
                } while (reader.Next());
            }

            if (wasMove)
                reader.Prev();

            return syntax;
        }
    }
}
