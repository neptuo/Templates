using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers.ComponentModel
{
    public class DefaultSourceContentInfo : ISourceContentInfo
    {
        public int StartIndex { get; private set; }
        public int Length { get; private set; }

        public DefaultSourceContentInfo(int startIndex, int length)
        {
            StartIndex = startIndex;
            Length = length;
        }

        public override string ToString()
        {
            return String.Format("<{0}, {1}>", StartIndex, StartIndex + Length);
        }
    }
}
