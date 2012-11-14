using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Extensions
{
    public class DateTimeExtension : IMarkupExtension
    {
        public object ProvideValue()
        {
            return DateTime.Now.ToString();
        }
    }
}
