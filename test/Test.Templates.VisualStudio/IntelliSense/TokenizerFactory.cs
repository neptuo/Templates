using Neptuo.Activators;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Templates.VisualStudio.IntelliSense
{
    public class TokenizerFactory : IFactory<ITokenizer>
    {
        public ITokenizer Create()
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
