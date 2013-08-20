using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleNG.Extensions
{
    public static class BindingManager
    {
        public static void SetValue(object target, string expression, object value)
        {
            PropertyInfo info = null;
            Type type = target.GetType();
            string[] exprs = expression.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < exprs.Length; i++)
            {
                info = type.GetProperty(exprs[i]);
                type = info.PropertyType;

                if (i < (exprs.Length - 1))
                    target = info.GetValue(target, null);
            }

            if (info != null)
                info.SetValue(target, value, null);
        }

        public static object GetValue(string expression, object value)
        {
            if (String.IsNullOrEmpty(expression))
                return value;

            if (value == null)
                return null;

            PropertyInfo info = null;
            Type type = value.GetType();
            string[] exprs = expression.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < exprs.Length; i++)
            {
                info = type.GetProperty(exprs[i]);
                if (info == null)
                    return null;

                type = info.PropertyType;

                if (value != null)
                    value = info.GetValue(value, null);
            }

            return value;
        }
    }
}
