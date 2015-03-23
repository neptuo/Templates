using Neptuo.Templates.Compilation.Parsers.Tokenizers.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers
{
    internal class CurlyTokenizerContext
    {
        public StringBuilder CurrentText { get; private set; }
        public IContentReader Reader { get; private set; }
        public ITokenizerContext TokenizerContext { get; private set; }
        public IList<CurlyToken> Tokens { get; private set; }

        public CurlyTokenizerContext(IContentReader reader, ITokenizerContext tokenizerContext)
        {
            CurrentText = new StringBuilder();
            Reader = reader;
            TokenizerContext = tokenizerContext;
            Tokens = new List<CurlyToken>();
        }
    }
}
