using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers
{
    public class TokenEnumerator : IEnumerator<Token>
    {
        private readonly IList<Token> tokens;
        private readonly int startIndex;
        private int currentIndex;

        public TokenEnumerator(IList<Token> tokens, int startIndex)
        {
            this.tokens = tokens;
            this.startIndex = startIndex;
            this.currentIndex = startIndex;
        }

        public Token Current
        {
            get { return tokens[currentIndex]; }
        }

        public void Dispose()
        { }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        public bool MoveNext()
        {
            if(currentIndex + 1 < tokens.Count)
            {
                currentIndex++;
                return true;
            }

            return false;
        }

        public void Reset()
        {
            currentIndex = startIndex;
        }
    }
}
