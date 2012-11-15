using Neptuo.Web.Framework.Compilation.CodeGenerators;
using Neptuo.Web.Framework.Compilation.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    public interface IViewService
    {
        //IParserService ParserService { get; }
        //ICodeGeneratorService CodeGeneratorService { get; }

        //void SetCurrent(string name);
        IGeneratedView Process(string fileName);
    }
}
