using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers
{
    /// <summary>
    /// Token type for <see cref="ComposableToken"/>.
    /// </summary>
    public class ComposableTokenType
    {
        public string UniqueName { get; private set; }

        public ComposableTokenType(string uniqueName)
        {
            Ensure.NotNullOrEmpty(uniqueName, "uniqueName");
            UniqueName = uniqueName;
        }

        public override int GetHashCode()
        {
            return 5 ^ UniqueName.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            ComposableTokenType otherType = obj as ComposableTokenType;
            if(otherType == null)
                return false;

            return UniqueName.Equals(otherType.UniqueName);
        }
    }
}
