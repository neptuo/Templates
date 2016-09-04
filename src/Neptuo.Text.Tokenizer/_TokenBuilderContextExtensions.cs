using Neptuo.Text.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Text
{
    public static class _TokenBuilderContextExtensions
    {
        public static IList<Token> Tokenize(this ITokenBuilderContext context, ContentDecorator decorator)
        {
            Ensure.NotNull(context, "context");
            return context.Tokenize(decorator, decorator);
        }

        public static IList<Token> TokenizePartial(this ITokenBuilderContext context, ContentDecorator decorator, Func<char, bool> terminator)
        {
            Ensure.NotNull(context, "context");
            return context.Tokenize(ContentReader.Partial(decorator, terminator), decorator);
        }

        public static IList<Token> TokenizePartial(this ITokenBuilderContext context, ContentDecorator decorator, params char[] terminators)
        {
            Ensure.NotNull(context, "context");
            return context.Tokenize(ContentReader.Partial(decorator, terminators), decorator);
        }
    }
}
