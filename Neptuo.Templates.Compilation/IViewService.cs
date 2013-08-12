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
        object Process(string fileName, IViewServiceContext context);
        object ProcessContent(string viewContent, IViewServiceContext context);
    }
}
