using Neptuo.Templates.Compilation.CodeGenerators;
using Neptuo.Templates.Compilation.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation
{
    public interface IViewService
    {
        IGeneratedView Process(string fileName, IViewServiceContext context);
        IGeneratedView ProcessContent(string viewContent, IViewServiceContext context);
    }
}
