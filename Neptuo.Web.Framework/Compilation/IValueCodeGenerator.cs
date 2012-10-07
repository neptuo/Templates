using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using System.Reflection;

namespace Neptuo.Web.Framework.Compilation
{
    public interface IValueCodeGenerator
    {
        bool GenerateCode(string content, ValueGeneratorContext context);
    }

    public class ValueCodeGeneratorTarget
    {
        public CodeMemberField Field { get; set; }

        public PropertyInfo Property { get; set; }
    }
}
