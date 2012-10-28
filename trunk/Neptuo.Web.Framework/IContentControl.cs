using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework
{
    public interface IContentControl : IControl
    {
        ICollection<object> Content { get; set; }
    }
}
