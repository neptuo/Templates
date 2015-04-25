using Neptuo.Activators;
using Neptuo.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers
{
    /// <summary>
    /// Default implementation of <see cref="IComposableTokenizerContext"/>
    /// </summary>
    public class DefaultComposableTokenizerContext : DefaultTokenizerContext, IComposableTokenizerContext
    {
        public DefaultComposableTokenizerContext(ITokenizerContext context)
            : base(context.DependencyProvider, context.Errors)
        { }

        public IList<ComposableTokenType> Tokenize(IO.IContentReader reader, IComposableTokenizer initiator)
        {
            throw new NotImplementedException();
        }
    }
}
