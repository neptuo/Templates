using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers;
using Neptuo.Templates.Compilation.Parsers.ExtensionContent;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Neptuo.Templates.Compilation.Parsers
{
    public partial class ExtensionValueParser : IValueParser
    {
        private IMarkupExtensionBuilderRegistry builderRegistry;

        public ExtensionValueParser(IMarkupExtensionBuilderRegistry builderRegistry)
        {
            this.builderRegistry = builderRegistry;
        }

        public bool Parse(string content, IValueParserContext context)
        {
            bool parsed = false;

            Helper helper = new Helper(context, builderRegistry);
            helper.Parser.OnParsedItem = (e) => parsed = GenerateExtension(helper, e.ParsedItem);
            helper.Parser.Parse(content);

            return parsed;
        }

        private bool GenerateExtension(Helper helper, Extension extension)
        {
            IMarkupExtensionBuilder builder = helper.BuilderRegistry.GetMarkupExtensionBuilder(extension.Namespace, extension.Name);
            if (builder == null)
                return false;

            return builder.Parse(new MarkupExtensionBuilderContext(this, helper), extension);
        }
    }
}
