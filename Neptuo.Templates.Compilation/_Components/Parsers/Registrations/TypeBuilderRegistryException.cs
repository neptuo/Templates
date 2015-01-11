using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Exception thrown from <see cref="TypeBuilderRegistry"/>.
    /// </summary>
    [Serializable]
    public class TypeBuilderRegistryException : Exception
    {
        public TypeBuilderRegistryException(string message) 
            : base(message) 
        { }

        public TypeBuilderRegistryException(string message, Exception inner) 
            : base(message, inner) 
        { }

        protected TypeBuilderRegistryException(SerializationInfo info, StreamingContext context)
            : base(info, context) 
        { }
    }
}
