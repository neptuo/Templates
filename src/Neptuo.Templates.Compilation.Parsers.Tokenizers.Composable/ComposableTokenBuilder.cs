using Neptuo.Templates.Compilation.Parsers.Tokenizers.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers
{
    public class ComposableTokenBuilder
    {
        private readonly Token token;

        public ComposableTokenBuilder()
        { }

        public ComposableTokenBuilder(Token token)
        {
            Ensure.NotNull(token, "token");
            this.token = token;
        }

        public ComposableTokenBuilder WithError(string errorMessage)
        {
            if (token != null)
                token.Errors.Add(new DefaultErrorMessage(errorMessage));

            return this;
        }
    }
}
