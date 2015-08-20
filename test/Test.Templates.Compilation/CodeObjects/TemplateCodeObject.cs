using Neptuo.Identifiers;
using Neptuo.Linq.Expressions;
using Neptuo.Templates.Compilation.CodeGenerators;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Templates.UI;

namespace Test.Templates.Compilation.CodeObjects
{
    public class TemplateCodeObject : ComponentCodeObject
    {
        public TemplateCodeObject(Type type)
            : base(type)
        { }
    }

    public class CodeDomTemplateGenerator : CodeDomComponentObjectGenerator
    {
        public CodeDomTemplateGenerator(IUniqueNameProvider nameProvider)
            : base(nameProvider)
        { }
    }
}
