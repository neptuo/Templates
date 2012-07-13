using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Neptuo.Web.Html;
using System.ComponentModel;

namespace Neptuo.Web.Mvc.ViewEngine.MarkupExtensions
{
    [DefaultProperty("Property")]
    public class BindingExtension : IMarkupExtension
    {
        [Dependency]
        public ViewContext ViewContext { get; set; }

        public string Property { get; set; }

        public object ProvideValue()
        {
            object model = ViewContext.ViewData.Model;
            return model.GetType().GetProperty(Property).GetValue(model, null);
        }
    }
}
