using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace Neptuo.Web.Framework.Utils
{
    public class TypeHelper
    {
        public static string PropertyName<T>(Expression<Func<T, object>> propertyGetter)
        {
            return (propertyGetter.Body as MemberExpression ?? ((UnaryExpression)propertyGetter.Body).Operand as MemberExpression).Member.Name;
        }

        public static string MethodName<T, TResult>(Expression<Func<T, Func<TResult>>> propertyGetter)
        {
            return (((((UnaryExpression) propertyGetter.Body).Operand as MethodCallExpression).Arguments[2] as ConstantExpression).Value as MethodInfo).Name;
        }
    }

    class TypeHelperTest
    {
        public static void Test()
        {
            Console.WriteLine(TypeHelper.PropertyName<DomainObject>(d => d.Name));
            Console.WriteLine(TypeHelper.MethodName<DomainObject, string>(d => d.GetName));
        }
    }

    class DomainObject
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string GetName()
        {
            return Name;
        }
    }
}