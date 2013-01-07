using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Extensions
{
    [DefaultProperty("Path")]
    public class ViewExtension : IMarkupExtension
    {
        public string Path { get; set; }

        public ViewExtension()
        {

        }

        public object ProvideValue(IMarkupExtensionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
