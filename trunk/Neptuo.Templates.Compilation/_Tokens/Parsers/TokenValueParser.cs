using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers;
using Neptuo.Tokens;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Markup extension value parser.
    /// </summary>
    public partial class TokenValueParser : IValueParser
    {
        private readonly ITokenBuilderFactory builderFactory;

        public TokenValueParser(ITokenBuilderFactory builderFactory)
        {
            Guard.NotNull(builderFactory, "builderFactory");
            this.builderFactory = builderFactory;
        }

        public bool Parse(string content, IValueParserContext context)
        {
            bool parsed = false;

            Helper helper = new Helper(context, builderFactory);
            helper.Parser.OnParsedToken += (sender, e) => parsed = GenerateExtension(helper, e.Token);
            helper.Parser.Parse(content);

            return parsed;
        }

        private bool GenerateExtension(Helper helper, Token extension)
        {
            ITokenBuilder builder = helper.BuilderFactory.CreateBuilder(extension.Prefix, extension.Name);
            if (builder == null)
                return false;

            return builder.Parse(new TokenBuilderContext(this, helper), extension);
        }
    }
}
