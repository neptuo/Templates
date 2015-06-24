using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers
{
    public class ComposableTokenEnumerator : IEnumerator<ComposableToken>
    {
        private readonly IList<ComposableToken> tokens;
        private readonly int startIndex;
        private int currentIndex;

        public ComposableTokenEnumerator(IList<ComposableToken> tokens, int startIndex)
        {
            this.tokens = tokens;
            this.startIndex = startIndex;
            this.currentIndex = startIndex;
        }

        public ComposableToken Current
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
