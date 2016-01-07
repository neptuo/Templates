using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes
{
    public static class _TokenListReaderExtensions
    {
        public static void NextRequired(this TokenListReader reader)
        {
            Ensure.NotNull(reader, "reader");
            if (!reader.Next())
                throw new MissingNextTokenException();
        }

        public static void NextRequiredOfType(this TokenListReader reader, TokenType tokenType)
        {
            Ensure.NotNull(reader, "reader");
            Ensure.NotNull(tokenType, "tokenType");
            NextRequired(reader);
            CurrentRequiredOfType(reader, tokenType);
        }

        public static void Next(this TokenListReader reader, int count)
        {
            Ensure.NotNull(reader, "reader");

            for (int i = 0; i < count; i++)
                reader.Next();
        }

        public static void CurrentRequiredOfType(this TokenListReader reader, TokenType tokenType)
        {
            if (!IsCurrentOfType(reader, tokenType))
                throw new InvalidTokenTypeException(reader.Current, tokenType);
        }

        public static bool IsCurrentOfType(this TokenListReader reader, TokenType tokenType)
        {
            Ensure.NotNull(reader, "reader");
            Ensure.NotNull(tokenType, "tokenType");
            return reader.Current.Type == tokenType;
        }
    }
}
