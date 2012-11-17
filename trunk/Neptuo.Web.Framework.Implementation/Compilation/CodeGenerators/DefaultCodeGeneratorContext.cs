﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.CodeGenerators
{
    public class DefaultCodeGeneratorContext : ICodeGeneratorContext
    {
        public TextWriter Output { get; set; }

        public ICodeGeneratorService CodeGeneratorService { get; set; }
        public IServiceProvider ServiceProvider { get; set; }

        public DefaultCodeGeneratorContext(TextWriter output, ICodeGeneratorService codeGeneratorService, IServiceProvider serviceProvider)
        {
            Output = output;
            CodeGeneratorService = codeGeneratorService;
            ServiceProvider = serviceProvider;
        }
    }
}
