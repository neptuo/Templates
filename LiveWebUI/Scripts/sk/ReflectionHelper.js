﻿var Neptuo$Reflection$ReflectionHelper =
{
    fullname: "Neptuo.Reflection.ReflectionHelper",
    baseTypeName: "System.Object",
    staticDefinition:
    {
        GetAttribute$1: function (T, member) {
            var attrs = Cast(member.GetCustomAttributes$$Type$$Boolean(Typeof(T), true), Array);
            if (attrs.length == 1)
                return attrs[0];
            else
                return null;
        },
        GetTypesInNamespace: function (newNamespace) {
            var targetAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            var parts = newNamespace.Split$$Char$Array(",");
            if (parts.length == 2)
                targetAssembly = System.Reflection.Assembly.Load$$String(parts[1]);
            return System.Linq.Enumerable.ToList$1(System.Type.ctor, System.Linq.Enumerable.Where$1$$IEnumerable$1$$Func$2(System.Type.ctor, targetAssembly.GetTypes(), function (t) {
                return t.get_Namespace() == parts[0];
            }));
        },
        GetAnnotatedProperties$1: function (T, type) {
            var result = new System.Collections.Generic.List$1.ctor(System.Reflection.PropertyInfo.ctor);
            for (var $i2 = 0, $t2 = type.GetProperties(), $l2 = $t2.length, prop = $t2[$i2]; $i2 < $l2; $i2++, prop = $t2[$i2]) {
                if (prop.GetCustomAttributes$$Type$$Boolean(Typeof(T), true).length == 1)
                    result.Add(prop);
            }
            return result;
        },
        IsGenericType$2$$Type: function (TAssignable, TArgument, testedType) {
            return Neptuo.Reflection.ReflectionHelper.IsGenericType$$Type$$Type$$Type(testedType, Typeof(TAssignable), Typeof(TArgument));
        },
        IsGenericType$1$$Type: function (TAssignable, testedType) {
            return Neptuo.Reflection.ReflectionHelper.IsGenericType$$Type$$Type(testedType, Typeof(TAssignable));
        },
        IsGenericType$$Type$$Type$$Type: function (testedType, assignableType, argumentType) {
            if (argumentType == null)
                return false;
            return assignableType.IsAssignableFrom(testedType) && testedType.get_IsGenericType() && (testedType.GetGenericArguments()[0].IsAssignableFrom(argumentType) || argumentType.IsAssignableFrom(testedType.GetGenericArguments()[0]));
        },
        IsGenericType$$Type$$Type: function (testedType, assignableType) {
            return assignableType.IsAssignableFrom(testedType) && testedType.get_IsGenericType();
        },
        GetGenericArgument: function (type, index) {
            if (!type.get_IsGenericType())
                return null;
            return type.GetGenericArguments()[index];
        },
        CanBeUsedInMarkup: function (type, requireDefaultCtor) {
            if (type.get_IsInterface())
                return false;
            if (type.get_IsAbstract())
                return false;
            if (requireDefaultCtor) {
                if (System.Reflection.ConstructorInfo.op_Equality$$ConstructorInfo$$ConstructorInfo(type.GetConstructor$$Type$Array(new Array(0)), null))
                    return false;
            }
            return true;
        },
        PropertyName$$Expression$1: function (propertyGetter) {
            return (As(propertyGetter.get_Body(), System.Linq.Expressions.MemberExpression.ctor) != null ? As(propertyGetter.get_Body(), System.Linq.Expressions.MemberExpression.ctor) : As((Cast(propertyGetter.get_Body(), System.Linq.Expressions.UnaryExpression.ctor)).get_Operand(), System.Linq.Expressions.MemberExpression.ctor)).get_Member().get_Name();
        },
        PropertyName$1$$Expression$1: function (T, propertyGetter) {
            return (As(propertyGetter.get_Body(), System.Linq.Expressions.MemberExpression.ctor) != null ? As(propertyGetter.get_Body(), System.Linq.Expressions.MemberExpression.ctor) : As((Cast(propertyGetter.get_Body(), System.Linq.Expressions.UnaryExpression.ctor)).get_Operand(), System.Linq.Expressions.MemberExpression.ctor)).get_Member().get_Name();
        },
        PropertyName$2$$Expression$1: function (T, TResult, propertyGetter) {
            return (As(propertyGetter.get_Body(), System.Linq.Expressions.MemberExpression.ctor) != null ? As(propertyGetter.get_Body(), System.Linq.Expressions.MemberExpression.ctor) : As((Cast(propertyGetter.get_Body(), System.Linq.Expressions.UnaryExpression.ctor)).get_Operand(), System.Linq.Expressions.MemberExpression.ctor)).get_Member().get_Name();
        },
        MethodName$2$$Expression$1: function (T, TParam1, propertyGetter) {
            return (As((As((As((Cast(propertyGetter.get_Body(), System.Linq.Expressions.UnaryExpression.ctor)).get_Operand(), System.Linq.Expressions.MethodCallExpression.ctor)).get_Arguments().get_Item$$Int32(2), System.Linq.Expressions.ConstantExpression.ctor)).get_Value(), System.Reflection.MethodInfo.ctor)).get_Name();
        },
        MethodName$3$$Expression$1: function (T, TParam1, TParam2, propertyGetter) {
            return (As((As((As((Cast(propertyGetter.get_Body(), System.Linq.Expressions.UnaryExpression.ctor)).get_Operand(), System.Linq.Expressions.MethodCallExpression.ctor)).get_Arguments().get_Item$$Int32(2), System.Linq.Expressions.ConstantExpression.ctor)).get_Value(), System.Reflection.MethodInfo.ctor)).get_Name();
        },
        MethodName$4$$Expression$1: function (T, TParam1, TParam2, TParam3, propertyGetter) {
            return (As((As((As((Cast(propertyGetter.get_Body(), System.Linq.Expressions.UnaryExpression.ctor)).get_Operand(), System.Linq.Expressions.MethodCallExpression.ctor)).get_Arguments().get_Item$$Int32(2), System.Linq.Expressions.ConstantExpression.ctor)).get_Value(), System.Reflection.MethodInfo.ctor)).get_Name();
        },
        MethodName$1$$Expression$1: function (T, propertyGetter) {
            return (As((As((As((Cast(propertyGetter.get_Body(), System.Linq.Expressions.UnaryExpression.ctor)).get_Operand(), System.Linq.Expressions.MethodCallExpression.ctor)).get_Arguments().get_Item$$Int32(2), System.Linq.Expressions.ConstantExpression.ctor)).get_Value(), System.Reflection.MethodInfo.ctor)).get_Name();
        },
        MethodName$5$$Expression$1: function (T, TParam1, TParam2, TParam3, TParam4, propertyGetter) {
            return (As((As((As((Cast(propertyGetter.get_Body(), System.Linq.Expressions.UnaryExpression.ctor)).get_Operand(), System.Linq.Expressions.MethodCallExpression.ctor)).get_Arguments().get_Item$$Int32(2), System.Linq.Expressions.ConstantExpression.ctor)).get_Value(), System.Reflection.MethodInfo.ctor)).get_Name();
        }
    },
    assemblyName: "TestSharpKit",
    Kind: "Class",
    definition:
    {
        ctor: function () {
            System.Object.ctor.call(this);
        }
    }
};
JsTypes.push(Neptuo$Reflection$ReflectionHelper);