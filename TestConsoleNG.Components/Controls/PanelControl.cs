using Neptuo.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestConsoleNG.Controls
{
    [Html("div")]
    public class PanelControl : BaseContentControl
    {
        public PanelControl(IComponentManager componentManager)
            : base(componentManager)
        { }
    }
}
