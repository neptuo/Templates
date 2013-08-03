﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators.Extensions.CodeDom
{
    public class BaseStructure
    {
        public CodeCompileUnit Unit { get; set; }
        public CodeTypeDeclaration Class { get; set; }
        public CodeMemberMethod EntryPointMethod { get; set; }
    }
}
