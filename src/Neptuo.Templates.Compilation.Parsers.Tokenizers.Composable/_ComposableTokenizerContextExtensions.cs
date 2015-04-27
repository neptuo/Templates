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
        public static IList<ComposableToken> Tokenize(this IComposableTokenizerContext context, ContentDecorator decorator)
        {
            Ensure.NotNull(context, "context");
            return context.Tokenize(decorator, decorator);
        }

        public static IList<ComposableToken> TokenizePartial(this IComposableTokenizerContext context, ContentDecorator decorator, Func<char, bool> terminator)
        {
            Ensure.NotNull(context, "context");
            return context.Tokenize(ContentReader.Partial(decorator, terminator), decorator);
        }

        public static IList<ComposableToken> TokenizePartial(this IComposableTokenizerContext context, ContentDecorator decorator, params char[] terminators)
        {
            Ensure.NotNull(context, "context");
            return context.Tokenize(ContentReader.Partial(decorator, terminators), decorator);
        }
    }
}
