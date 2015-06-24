using Neptuo.Templates.Compilation.Parsers.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.SyntaxTrees
{
    public static class _TokenListReaderExtensions
    {
        public static void NextRequired(this TokenListReader reader)
        {
            Ensure.NotNull(reader, "reader");
            if (!reader.Next())
                throw Ensure.Exception.NotSupported();
        }

        public static void NextRequiredOfType(this TokenListReader reader, ComposableTokenType tokenType)
        {
            Ensure.NotNull(reader, "reader");
            Ensure.NotNull(tokenType, "tokenType");
            NextRequired(reader);

            if (reader.Current.Type != tokenType)
                throw Ensure.Exception.NotSupported();
        }
    }
}
