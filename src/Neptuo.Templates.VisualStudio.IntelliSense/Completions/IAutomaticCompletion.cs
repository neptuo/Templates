using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio.IntelliSense.Completions
{
    /// <summary>
    /// Describes automatic text replacement action.
    /// </summary>
    public interface IAutomaticCompletion
    {
        /// <summary>
        /// Text to add.
        /// </summary>
        string Text { get; }

        /// <summary>
        /// Relative text insertion to the current token start index.
        /// </summary>
        RelativePosition InsertPosition { get; }

        /// <summary>
        /// Relative cursor position to the end of <see cref="IAutomaticCompletion.Text"/>.
        /// </summary>
        RelativePosition CursorPosition { get; }
    }
}
