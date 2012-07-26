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

        public static string PropertyName<T, TResult>(Expression<Func<T, TResult>> propertyGetter)
        {
            return (propertyGetter.Body as MemberExpression ?? ((UnaryExpression)propertyGetter.Body).Operand as MemberExpression).Member.Name;
        }

        public static string MethodName<T, TResult>(Expression<Func<T, Func<TResult>>> propertyGetter)
        {
            return (((((UnaryExpression)propertyGetter.Body).Operand as MethodCallExpression).Arguments[2] as ConstantExpression).Value as MethodInfo).Name;
        }

        public static string MethodName<T>(Expression<Func<T, Action>> propertyGetter)
        {
            return (((((UnaryExpression)propertyGetter.Body).Operand as MethodCallExpression).Arguments[2] as ConstantExpression).Value as MethodInfo).Name;
        }
    }
}