using Neptuo.Templates.Compilation.Parsers.Tokenizers.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers
{
    public static class _ComposableTokenizerContextExtensions
    {
        public static IList<ComposableToken> Tokenize(this IComposableTokenizerContext context, ContentDecorator decorator, IComposableTokenizer initiator)
        {
            Ensure.NotNull(context, "context");
            return context.Tokenize(decorator, decorator, initiator);
        }

        public static IList<ComposableToken> TokenizePartial(this IComposableTokenizerContext context, ContentDecorator decorator, Func<char, bool> terminator, IComposableTokenizer initiator)
        {
            Ensure.NotNull(context, "context");
            return context.Tokenize(ContentReader.Partial(decorator, terminator), decorator, initiator);
        }
    }
}
