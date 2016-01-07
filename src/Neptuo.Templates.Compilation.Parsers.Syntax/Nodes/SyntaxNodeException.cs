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
    public class SyntaxNodeException : Exception
    {
        public SyntaxNodeException() 
        { }

        public SyntaxNodeException(string message) 
            : base(message) 
        { }

        public SyntaxNodeException(string message, Exception inner) 
            : base(message, inner) 
        { }

        protected SyntaxNodeException(SerializationInfo info, StreamingContext context)
            : base(info, context) 
        { }
    }
}
