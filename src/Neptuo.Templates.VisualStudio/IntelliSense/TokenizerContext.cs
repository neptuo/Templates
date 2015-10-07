using Microsoft.VisualStudio.Text;
using Neptuo.Activators;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio.IntelliSense
{
    public class TokenizerContext
    {
        private readonly DefaultTokenizer tokenizer;
        private readonly IDependencyContainer dependencyContainer;

        public TokenizerContext()
        {
            this.tokenizer = new DefaultTokenizer();
            tokenizer.Add(new CurlyTokenBuilder());
            tokenizer.Add(new LiteralTokenBuilder());

            this.dependencyContainer = new UnityDependencyContainer();
        }

        public IList<Token> Tokenize(ITextBuffer textBuffer)
        {
            string textContent = textBuffer.CurrentSnapshot.GetText();
            return tokenizer.Tokenize(new StringReader(textContent), new DefaultTokenizerContext(dependencyContainer));
        }
    }
}
