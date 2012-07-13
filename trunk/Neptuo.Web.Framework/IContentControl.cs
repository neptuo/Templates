using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework
{
    public interface IContentControl : IControl
    {
        List<object> Content { get; set; }
    }
}
