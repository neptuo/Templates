using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Templates.Controls
{
    public interface IContentControl : IControl
    {
        ICollection<object> Content { get; set; }
    }
}
