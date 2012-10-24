using Neptuo.Web.Framework.Compilation.CodeObjects;
using Neptuo.Web.Framework.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    public partial class BaseParser
    {
        protected bool BindPropertyDefaultValue(IPropertyDescriptor propertyDescriptor)
        {
            //DependencyAttribute dependency = DependencyAttribute.GetAttribute(prop);
            //if (dependency != null)
            //{
            //    creator.SetProperty(prop.Name, helper.Context.CodeGenerator.GetDependencyFromServiceProvider(prop.PropertyType));
            //}
            //else
            //{
            DefaultValueAttribute defaultValue = ReflectionHelper.GetAttribute<DefaultValueAttribute>(propertyDescriptor.Property);
            if (defaultValue != null)
            {
                propertyDescriptor.SetValue(new PlainValueCodeObject(defaultValue.Value));
                return true;
            }
            //}
            return false;
        }
    }
}
