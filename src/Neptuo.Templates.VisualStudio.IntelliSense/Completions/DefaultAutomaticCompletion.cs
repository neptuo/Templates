using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio.IntelliSense.Completions
{
    /// <summary>
    /// Default implementation of <see cref="IAutomaticCompletion"/>.
    /// </summary>
    public class DefaultAutomaticCompletion : IAutomaticCompletion
    {
        public string Text { get; private set; }
        public RelativePosition InsertPosition { get; private set; }
        public RelativePosition CursorPosition { get; private set; }

        public DefaultAutomaticCompletion(string text, RelativePosition insertPosition, RelativePosition cursorPosition)
        {
            Ensure.NotNullOrEmpty(text, "text");
            Ensure.NotNull(insertPosition, "insertPosition");
            Ensure.NotNull(cursorPosition, "cursorPosition");
            Text = text;
            InsertPosition = insertPosition;
            CursorPosition = cursorPosition;
        }
    }
}
