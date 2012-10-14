using Neptuo.Web.Framework.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    partial class BaseParser
    {
        public interface IPropertyInfo
        {
            Type RequiredType { get; }
            void SetProperty(CodeObject parentObject, ICodeObject codeObject);
        }

        public class SetPropertyInfo : IPropertyInfo
        {
            private string propertyName;

            public Type RequiredType { get; protected set; }

            public SetPropertyInfo(PropertyInfo propertyInfo)
            {
                this.propertyName = propertyInfo.Name;
                RequiredType = propertyInfo.PropertyType;
            }

            public void SetProperty(CodeObject parentObject, ICodeObject codeObject)
            {
                parentObject.Properties.Add(propertyName, codeObject);
            }
        }

        public class ListAddPropertyInfo : IPropertyInfo
        {
            private string propertyName;
            //private string addMethodName;

            public Type RequiredType { get; protected set; }

            public ListAddPropertyInfo(PropertyInfo propertyInfo)
            {
                this.propertyName = propertyInfo.Name;
                //this.addMethodName = TypeHelper.MethodName<IList, object, int>(l => l.Add);
                RequiredType = ReflectionHelper.GetGenericArgument(propertyInfo.PropertyType);
            }

            public void SetProperty(CodeObject parentObject, ICodeObject codeObject)
            {
                if (!parentObject.Properties.ContainsKey(propertyName))
                    parentObject.Properties.Add(propertyName, new ListValueCodeObject());

                ((ListValueCodeObject)parentObject.Properties[propertyName]).Values.Add(codeObject);
            }
        }


    }
}
