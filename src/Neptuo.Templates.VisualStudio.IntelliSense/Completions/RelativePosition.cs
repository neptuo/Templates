using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio.IntelliSense.Completions
{
    /// <summary>
    /// Describes relative postion in text.
    /// </summary>
    public class RelativePosition
    {
        public int Value { get; private set; }

        public RelativePosition(int value)
        {
            Value = value;
        }

        public static RelativePosition Start()
        {
            return new RelativePosition(0);
        }

        public static RelativePosition End(string text)
        {
            Ensure.NotNull(text, "text");
            return new RelativePosition(text.Length);
        }
    }
}
