using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Describes context for property descriptor sub generator.
    /// </summary>
    public class CodeDomPropertyContext
    {
        /// <summary>
        /// Innner context of code dom generator.
        /// </summary>
        public CodeDomGenerator.Context Context { get; private set; }

        /// <summary>
        /// Current instance of code dom generator.
        /// </summary>
        public CodeDomGenerator CodeGenerator { get; private set; }

        /// <summary>
        /// Field name where property descriptor comes from.
        /// Can <c>null</c> for root property.
        /// </summary>
        public string FieldName { get; private set; }

        /// <summary>
        /// Type of <see cref="CodeDomPropertyContext.FieldName"/>.
        /// </summary>
        public Type FieldType { get; private set; }

        /// <summary>
        /// Collection of statements in current method.
        /// </summary>
        public CodeStatementCollection Statements { get; private set; }

        public CodeDomPropertyContext(CodeDomGenerator.Context context, string fieldName, Type fieldType, CodeStatementCollection statements)
        {
            Guard.NotNull(context, "context");
            CodeGenerator = context.CodeGenerator;
            Context = context;
            FieldName = fieldName;
            FieldType = fieldType;
            Statements = statements;
        }
    }
}
