using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Controls
{
    [Html("div")]
    public class PanelControl : BaseContentControl
    {
        public PanelControl()
        {
            Content = new List<object>();
        }
    }
}
