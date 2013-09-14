using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers;
using Neptuo.Tokens;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Neptuo.Templates.Compilation.Parsers
{
    public partial class MarkupExtensionValueParser : IValueParser
    {
        private IMarkupExtensionBuilderRegistry builderRegistry;

        public MarkupExtensionValueParser(IMarkupExtensionBuilderRegistry builderRegistry)
        {
            this.builderRegistry = builderRegistry;
        }

        public bool Parse(string content, IValueParserContext context)
        {
            bool parsed = false;

            Helper helper = new Helper(context, builderRegistry);
            helper.Parser.OnParsedToken = (e) => parsed = GenerateExtension(helper, e.Token);
            helper.Parser.Parse(content);

            return parsed;
        }

        private bool GenerateExtension(Helper helper, Token extension)
        {
            IMarkupExtensionBuilder builder = helper.BuilderRegistry.GetMarkupExtensionBuilder(extension.Prefix, extension.Name);
            if (builder == null)
                return false;

            return builder.Parse(new MarkupExtensionBuilderContext(this, helper), extension);
        }
    }
}
