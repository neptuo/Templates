using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Extensions
{
    public class BindingExtension : IMarkupExtension
    {
        public object ProvideValue()
        {
            return "Hello, World!";
        }
    }
}
