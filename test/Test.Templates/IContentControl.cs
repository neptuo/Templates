using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.Templates
{
    public interface IContentControl : IControl
    {
        ICollection<object> Content { get; set; }
    }
}
