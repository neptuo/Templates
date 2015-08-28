using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Templates.Metadata;

namespace Test.Templates.UI
{
    [ReturnType(typeof(string))]
    [DefaultProperty("Format")]
    public class DateTimeExtension : IValueExtension
    {
        public string Format { get; set; }

        public object ProvideValue(IValueExtensionContext context)
        {
            if (Format == null)
                Format = "{0}";
            else
                Format = "{0:" + Format + "}";

            return String.Format(Format, DateTime.Now);
        }
    }
}
