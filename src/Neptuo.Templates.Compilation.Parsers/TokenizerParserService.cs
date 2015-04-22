using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Implementation of <see cref="IParserService"/> that uses tokenizers.
    /// </summary>
    public class TokenizerParserService : IParserService
    {
        public ICodeObject ProcessContent(string name, ISourceContent content, IParserServiceContext context)
        {
            throw new NotImplementedException();
        }

        public ICodeObject ProcessValue(string name, ISourceContent value, IParserServiceContext context)
        {
            throw new NotImplementedException();
        }
    }
}
