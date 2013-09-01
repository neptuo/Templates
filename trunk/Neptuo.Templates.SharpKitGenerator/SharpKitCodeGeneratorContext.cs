using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.SharpKitGenerator
{
    public class SharpKitCodeGeneratorContext
    {
        public TextReader Input { get; private set; }
        public TextWriter Output { get; private set; }

        public SharpKitCodeGeneratorContext(TextReader input, TextWriter output)
        {
            Input = input;
            Output = output;
        }
    }
}
