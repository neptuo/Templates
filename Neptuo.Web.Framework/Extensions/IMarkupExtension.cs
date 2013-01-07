using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Extensions
{
    public interface IMarkupExtension
    {
        object ProvideValue(IMarkupExtensionContext context);
    }
}
