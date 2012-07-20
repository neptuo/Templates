using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neptuo.Web.Framework.Annotations;

namespace Neptuo.Web.Framework.Controls
{
    [Control(TagName = "div")]
    [ControlBuilder(typeof(PanelControlBuilder))]
    public class PanelControl : BaseContentControl
    {
        public PanelControl()
        {
            Content = new List<object>();
        }
    }
}
