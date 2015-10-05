using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Common extensions for <see cref="IParserProvider"/>.
    /// </summary>
    public static class _ParserCollectionExtensions
    {
        public static ICodeObjectBuilder WithObjectBuilder(this IParserProvider model)
        {
            Ensure.NotNull(model, "model");
            return model.With<ICodeObjectBuilder>();
        }

        public static ICodePropertyBuilder WithPropertyBuilder(this IParserProvider model)
        {
            Ensure.NotNull(model, "model");
            return model.With<ICodePropertyBuilder>();
        }
    }
}
