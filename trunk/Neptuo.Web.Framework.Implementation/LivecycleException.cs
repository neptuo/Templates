using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework
{
    [Serializable]
    public class LivecycleException : Exception
    {
        public LivecycleException() { }
        public LivecycleException(string message) : base(message) { }
        public LivecycleException(string message, Exception inner) : base(message, inner) { }
        protected LivecycleException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
