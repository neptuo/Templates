using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Usefull extensions for <see cref="System.CodeDom"/>.
    /// </summary>
    public static class _CodeDomExtensions
    {
        public static CodeStatementCollection AddRange(this CodeStatementCollection collection, IEnumerable<CodeStatement> statements)
        {
            Guard.NotNull(collection, "collection");
            Guard.NotNull(statements, "statements");

            foreach (CodeStatement statement in statements)
                collection.Add(statement);

            return collection;
        }


        #region ReturnType

        public static CodeExpression AddReturnType(this CodeExpression expression, Type returnType)
        {
            Guard.NotNull(expression, "expression");
            Guard.NotNull(returnType, "returnType");
            expression.UserData["ReturnType"] = returnType;
            return expression;
        }

        public static bool TryGetReturnType(this CodeExpression expression, out Type returnType)
        {
            if (expression.UserData.Contains("ReturnType"))
                returnType = expression.UserData["ReturnType"] as Type;
            else
                returnType = null;

            return returnType != null;
        }

        public static Type GetReturnType(this CodeExpression expression)
        {
            Type returnType;
            if (TryGetReturnType(expression, out returnType))
                return returnType;

            throw Guard.Exception.InvalidOperation("Return type was not specified on expression.");
        }

        #endregion
    }
}
