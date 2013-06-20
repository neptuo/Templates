using Neptuo.Web.Framework.Extensions;
using Neptuo.Web.Framework.Mvc.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Web.Framework.Mvc.Extensions
{
    [DefaultProperty("Path")]
    public class BindingExtension : IMarkupExtension
    {
        private DataStorage storage;

        public string Path { get; set; }

        public BindingExtension(DataStorage storage)
        {
            this.storage = storage;
        }

        public object ProvideValue(IMarkupExtensionContext context)
        {
            object data = storage.Peek();
            if (data == null)
                return null;

            if (String.IsNullOrEmpty(Path))
                return data;

            Type dataType = data.GetType();
            PropertyInfo property = dataType.GetProperty(Path);
            if (property == null)
                return null;

            object value = property.GetGetMethod().Invoke(data, null);
            if (value != null)
                return value.ToString();

            return value;
        }
    }
}
