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

            IEnumerator<ComposableToken> enumerator = new ComposableTokenEnumerator(tokens, startIndex);
            ComposableToken token = enumerator.Current;
            if (token.Type == CurlyTokenType.OpenBrace)
            {
                result = result.WithOpenToken(token);
                return BuildName(enumerator, result);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        private ISyntaxNode BuildName(IEnumerator<ComposableToken> enumerator, CurlySyntax result)
        {
            if (enumerator.MoveNext())
            {
                CurlyNameSyntax name = CurlyNameSyntax.New();
                if (enumerator.Current.Type == CurlyTokenType.NamePrefix)
                {
                    name = name.WithPrefix(enumerator.Current);
                    if (enumerator.MoveNext() && enumerator.Current.Type == CurlyTokenType.NameSeparator)
                    {
                        name = name.WithNameSeparator(enumerator.Current);
                        if (enumerator.MoveNext() && enumerator.Current.Type == CurlyTokenType.Name)
                            name = name.WithName(enumerator.Current);
                        else
                            throw new NotImplementedException();
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                else if (enumerator.Current.Type == CurlyTokenType.Name)
                {
                    name = name.WithName(enumerator.Current);
                }
                else
                {
                    throw new NotImplementedException();
                }

                result = result.WithName(name);
                return BuildContent(enumerator, result);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private ISyntaxNode BuildContent(IEnumerator<ComposableToken> enumerator, CurlySyntax result)
        {
            if (enumerator.MoveNext())
            {
                if(enumerator.Current.Type == CurlyTokenType.CloseBrace)
                {
                    result = result.WithCloseToken(enumerator.Current);
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
    }
}
