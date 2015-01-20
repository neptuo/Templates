using Neptuo.Templates.Compilation.CodeGenerators;
using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Templates.Compilation.CodeObjects
{
    public class MethodReferenceCodeObject : ICodeObject
    {
        public string MethodName { get; set; }

        public MethodReferenceCodeObject(string methodName)
        {
            MethodName = methodName;
        }
    }

    public class CodeDomMethodReferenceGenerator : XCodeDomObjectGeneratorBase<MethodReferenceCodeObject>
    {
        protected override CodeExpression GenerateCode(CodeObjectExtensionContext context, MethodReferenceCodeObject codeObject, ICodeProperty codeProperty)
        {
            CodeMemberMethod method = context.BaseStructure.Class.Members.FindMethod(codeObject.MethodName);
            if(method == null)
                return new CodePrimitiveExpression(null);

            return new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), codeObject.MethodName);
        }
    }

}
