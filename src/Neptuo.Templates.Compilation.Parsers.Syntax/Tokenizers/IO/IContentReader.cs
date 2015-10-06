using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers.IO
{
    /// <summary>
    /// Describes content reader.
    /// When new instance is created, reader points to the first character.
    /// When no character is available, <see cref="ContentReader.EndOfInput"/> is returned.
    /// </summary>
    public interface IContentReader
    {
        /// <summary>
        /// Current reader position/index.
        /// </summary>
        int Position { get; }

        /// <summary>
        /// Character at index <see cref="IContentReader.Position"/>.
        /// </summary>
        char Current { get; }

        /// <summary>
        /// Tries to read next character.
        /// Returns <c>true</c> if reading was successfull; <c>false</c> when end of file was reached.
        /// </summary>
        /// <returns><c>true</c> if reading was successfull; <c>false</c> when end of file was reached.</returns>
        bool Next();
    }
}
