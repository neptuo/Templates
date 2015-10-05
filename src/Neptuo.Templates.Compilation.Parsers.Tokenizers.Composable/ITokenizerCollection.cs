using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers
{
    /// <summary>
    /// Collection of composable tokenizers.
    /// </summary>
    public interface ITokenizerCollection
    {
        /// <summary>
        /// Adds <paramref name="tokenizer"/> to the collection.
        /// </summary>
        /// <param name="tokenizer">Tokenizer to add to the collection.</param>
        /// <returns>Self (for fluency).</returns>
        ITokenizerCollection Add(TokenizerBase tokenizer);
    }
}
