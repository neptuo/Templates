using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class SequenceFieldNameProvider : IFieldNameProvider
    {
        private int index = 0;

        public string GetName()
        {
            return String.Format("field{0}", ++index);
        }
    }
}
