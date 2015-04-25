using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class CurlyTokenBuilder
    {
        public IEnumerable<ICodeObject> TryParse(IList<ComposableToken> tokens)
        {
            throw Ensure.Exception.NotImplemented();
        }
    }
}
