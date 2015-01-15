using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public static class CodeTypeMemberCollectionExtensions
    {
        public static CodeMemberMethod FindMethod(this CodeTypeMemberCollection members, string methodName)
        {
            foreach (CodeTypeMember member in members)
            {
                CodeMemberMethod method = member as CodeMemberMethod;
                if (method != null && method.Name == methodName)
                    return method;
            }

            return null;
        }
    }
}
