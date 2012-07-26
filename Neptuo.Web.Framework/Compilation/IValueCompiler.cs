using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using System.Reflection;

namespace Neptuo.Web.Framework.Compilation
{
    public interface IValueCompiler
    {
        bool GenerateCode(string content, ValueCompilerContext context);
    }

    public class ValueCompilerTarget
    {
        public CodeMemberField Field { get; set; }

        public PropertyInfo Property { get; set; }
    }
}
