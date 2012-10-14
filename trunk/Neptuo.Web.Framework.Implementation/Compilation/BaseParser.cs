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
        protected void BindPropertyDefaultValue(CodeObject codeObject, PropertyInfo prop)
        {
            //DependencyAttribute dependency = DependencyAttribute.GetAttribute(prop);
            //if (dependency != null)
            //{
            //    creator.SetProperty(prop.Name, helper.Context.CodeGenerator.GetDependencyFromServiceProvider(prop.PropertyType));
            //}
            //else
            //{
            DefaultValueAttribute defaultValue = ReflectionHelper.GetAttribute<DefaultValueAttribute>(prop);
            if (defaultValue != null)
                codeObject.Properties.Add(prop.Name, new PlainValueCodeObject(defaultValue.Value));
            //}
        }
    }
}
