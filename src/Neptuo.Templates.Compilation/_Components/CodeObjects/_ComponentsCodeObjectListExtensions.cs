using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    public static class _ComponentsCodeObjectCollectionExtensions
    {
        /// <summary>
        /// Adds <see cref="LiteralCodeObject"/>.
        /// </summary>
        /// <param name="list">Target list of code objects.</param>
        /// <param name="literalType">Typ of literal control.</param>
        /// <param name="textProperty">Name of the text property.</param>
        /// <param name="value">Literal value.</param>
        /// <returns><paramref name="list"/>.</returns>
        public static CodeObjectCollection AddLiteral(this CodeObjectCollection list, Type literalType, string textProperty, object value)
        {
            Ensure.NotNull(list, "list");
            list.Add(new LiteralCodeObject(literalType, textProperty, value));
            return list;
        }
    }
}
