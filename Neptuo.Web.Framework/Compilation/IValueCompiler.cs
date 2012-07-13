using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    public interface IValueCompiler
    {
        void GenerateCode(object parsedItem, ValueCompilerContext context);
    }
}
