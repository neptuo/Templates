using Neptuo.Web.Framework.Compilation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Composition
{
    public class TemplateTypeConverter : TypeConverter
    {
        private IViewService viewService;
        private IDependencyProvider dependencyProvider;

        public TemplateTypeConverter(IViewService viewService, IDependencyProvider dependencyProvider)
        {
            this.viewService = viewService;
            this.dependencyProvider = dependencyProvider;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(string))
            {
                //Template template = new Template(dependencyProvider, viewService, value.ToString());
                //return template;
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
