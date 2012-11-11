using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Controls
{
    public class TemplateControl : BaseControl
    {
        public string Path { get; set; }
        public IEnumerable<ContentControl> Content { get; set; }
    }

    public class ContentControl : BaseContentControl
    {
        public string Name { get; set; }
    }
}
