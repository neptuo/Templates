using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    public interface IContentCompiler { }

    public interface IContentCompiler<T> : IContentCompiler
    {
        void GenerateCode(T parsedItem, ContentCompilerContext context);

        void AppendPlainText(string text, ContentCompilerContext context);
    }
}
