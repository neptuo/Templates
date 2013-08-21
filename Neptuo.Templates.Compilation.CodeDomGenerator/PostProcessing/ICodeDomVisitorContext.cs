﻿using Neptuo.Templates.Compilation.CodeGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.PostProcessing
{
    public interface ICodeDomVisitorContext
    {
        CodeDomGenerator CodeDomGenerator { get; }
        CodeDomGenerator.Context GeneratorContext { get; }
        BaseCodeDomStructure BaseStructure { get; }
    }
}