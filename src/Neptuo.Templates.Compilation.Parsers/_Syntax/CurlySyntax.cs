using Neptuo.Templates.Compilation.Parsers.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class CurlySyntax : ISyntaxNode
    {
        public IReadOnlyList<ComposableToken> Tokens { get; private set; }

        public ComposableToken OpenToken { get; private set; }
        public CurlyNameSyntax Name { get; private set; }
        public ComposableToken CloseToken { get; private set; }

        internal CurlySyntax(ComposableToken openToken, CurlyNameSyntax name, ComposableToken closeToken)
        {
            OpenToken = openToken;
            Name = name;
            CloseToken = closeToken;
            List<ComposableToken> tokens = new List<ComposableToken>();

            if (openToken != null)
                tokens.Add(openToken);

            if (name != null)
                tokens.AddRange(name.Tokens);

            if (closeToken != null)
                tokens.Add(closeToken);

            Tokens = tokens;
        }

        public CurlySyntax WithOpenToken(ComposableToken openToken)
        {
            return new CurlySyntax(openToken, Name, CloseToken);
        }

        public CurlySyntax WithName(CurlyNameSyntax name)
        {
            return new CurlySyntax(OpenToken, name, CloseToken);
        }

        public CurlySyntax WithCloseToken(ComposableToken closeToken)
        {
            return new CurlySyntax(OpenToken, Name, closeToken);
        }



        public static CurlySyntax New()
        {
            return new CurlySyntax(null, null, null);
        }
    }

    public class CurlyNameSyntax : ISyntaxNode
    {
        public IReadOnlyList<ComposableToken> Tokens { get; private set; }
        
        public ComposableToken Prefix { get; private set; }
        public ComposableToken NameSeparator { get; private set; }
        public ComposableToken Name { get; private set; }

        internal CurlyNameSyntax(ComposableToken prefix, ComposableToken nameSeparator, ComposableToken name)
        {
            Prefix = prefix;
            NameSeparator = nameSeparator;
            Name = name;

            List<ComposableToken> tokens = new List<ComposableToken>();
            if (prefix != null)
                tokens.Add(prefix);

            if (nameSeparator != null)
                tokens.Add(nameSeparator);

            if (name != null)
                tokens.Add(name);

            Tokens = tokens;
        }

        public CurlyNameSyntax WithPrefix(ComposableToken prefix)
        {
            return new CurlyNameSyntax(prefix, NameSeparator, Name);
        }

        public CurlyNameSyntax WithNameSeparator(ComposableToken nameSeparator)
        {
            return new CurlyNameSyntax(Prefix, nameSeparator, Name);
        }

        public CurlyNameSyntax WithName(ComposableToken name)
        {
            return new CurlyNameSyntax(Prefix, NameSeparator, name);
        }


        public static CurlyNameSyntax New()
        {
            return new CurlyNameSyntax(null, null, null);
        }
    }
}
