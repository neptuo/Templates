using Microsoft.VisualStudio.Text;
using Neptuo.Activators;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using Neptuo.Templates.VisualStudio.IntelliSense;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Templates.VisualStudio.IntelliSense
{
    public class TokenizerFactory : ITokenizerFactory
    {
        public ITokenizer Create(ITextBuffer context)
        {
            DefaultTokenizer tokenizer = new DefaultTokenizer();
            tokenizer
                .Add(new AngleTokenBuilder())
                .Add(new CurlyTokenBuilder())
                .Add(new LiteralTokenBuilder());

            return tokenizer;
        }
    }
}
