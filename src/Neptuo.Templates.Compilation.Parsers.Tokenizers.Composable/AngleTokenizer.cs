using Neptuo.Templates.Compilation.Parsers.Tokenizers.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers
{
    public class AngleTokenizer : TokenizerBase
    {
        protected override void Tokenize(ContentDecorator decorator, IComposableTokenizerContext context, List<ComposableToken> result)
        {
            if (decorator.Current != '<')
            {
                IList<ComposableToken> tokens = context.TokenizePartial(decorator, '<', '>');
                result.AddRange(tokens);
            }

            if (decorator.Current == '<')
                ChooseNodeType(decorator, context, result);

            throw Ensure.Exception.NotImplemented();
        }

        private bool ChooseNodeType(ContentDecorator decorator, IComposableTokenizerContext context, List<ComposableToken> result)
        {
            if (!decorator.Next())
                return false;

            if (Char.IsLetter(decorator.Current))
            {
                CreateToken(decorator, result, AngleTokenType.OpenBrace)
                return ReadElementName(decorator, context, result);
            }
            else if (decorator.Current == '!')
            {
                return ReadDirectiveName(decorator, context, result);
            }

            throw Ensure.Exception.NotImplemented();
        }

        private bool ReadElementName(ContentDecorator decorator, IComposableTokenizerContext context, List<ComposableToken> result)
        {
            throw Ensure.Exception.NotImplemented();
        }

        private bool ReadDirectiveName(ContentDecorator decorator, IComposableTokenizerContext context, List<ComposableToken> result)
        {
            throw Ensure.Exception.NotImplemented();
        }
    }
}
