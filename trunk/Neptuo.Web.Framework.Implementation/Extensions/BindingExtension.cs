using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Neptuo.Web.Framework.Extensions
{
    [DefaultProperty("Expression")]
    public class BindingExtension : IMarkupExtension
    {
        [DefaultValue("Joe")]
        public string Expression { get; set; }

        public object ProvideValue()
        {
            if (!Models.CurrentModel.Any())
                return null;

            return Models.CurrentModel.Peek();
        }
    }
}
