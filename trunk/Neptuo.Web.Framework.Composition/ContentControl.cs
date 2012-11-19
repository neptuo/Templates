using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Composition
{
    [DefaultProperty("Content")]
    public class ContentControl
    {
        public string Name { get; set; }
        public ICollection<object> Content { get; set; }
    }
}
