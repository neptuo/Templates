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

    public class CodeDomMethodReferenceGenerator : CodeDomObjectGeneratorBase<MethodReferenceCodeObject>
    {
        protected override ICodeDomObjectResult Generate(ICodeDomObjectContext context, MethodReferenceCodeObject codeObject)
        {
            throw new NotImplementedException();
        }
    }

}
