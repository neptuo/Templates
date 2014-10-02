using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers;
using Neptuo.Tokens;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Token value parser.
    /// </summary>
    public partial class TokenValueParser : IValueParser
    {
        private readonly ITokenBuilderFactory builderFactory;

        public TokenValueParser(ITokenBuilderFactory builderFactory)
        {
            Guard.NotNull(builderFactory, "builderFactory");
            this.builderFactory = builderFactory;
        }

        public ICodeObject Parse(string content, IValueParserContext context)
        {
            ICodeObject codeObject = null;

            Helper helper = new Helper(context, builderFactory);
            helper.Parser.OnParsedToken += (sender, e) => codeObject = GenerateToken(helper, e.Token);
            helper.Parser.Parse(content);

            return codeObject;
        }

        private ICodeObject GenerateToken(Helper helper, Token extension)
        {
            ITokenBuilder builder = helper.BuilderFactory.CreateBuilder(extension.Prefix, extension.Name);
            if (builder == null)
                return null;

            ICodeObject codeObject = builder.Parse(new TokenBuilderContext(this, helper), extension);
            return codeObject;
        }
    }
}
