using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes
{
    /// <summary>
    /// Base exception describing error in processing syntax nodes.
    /// </summary>
    [Serializable]
    public class NodeException : Exception
    {
        public NodeException() 
        { }

        public NodeException(string message) 
            : base(message) 
        { }

        public NodeException(string message, Exception inner) 
            : base(message, inner) 
        { }

        protected NodeException(SerializationInfo info, StreamingContext context)
            : base(info, context) 
        { }
    }
}
