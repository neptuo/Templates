using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neptuo.Web.Framework.Parser;
using System.CodeDom;

namespace Neptuo.Web.Framework.CompilationOld
{
    public class ContentCompilerContext
    {
        public CodeGenerator CodeGenerator { get; set; }

        public CompilerContext CompilerContext { get; set; }

        public IServiceProvider ServiceProvider { get; set; }

        public IContentParser Parser { get; set; }

        public ParentInfo ParentInfo { get; set; }
    }

    public class ParentInfo
    {
        public string MemberName { get; set; }

        public string PropertyName { get; set; }

        public string MethodName { get; set; }

        public Type RequiredType { get; set; }

        public ParentInfo(string memberName, string propertyName, string methodName, Type requiredType)
        {
            MemberName = memberName;
            PropertyName = propertyName;
            MethodName = methodName;
            RequiredType = requiredType;
        }
    }
}
