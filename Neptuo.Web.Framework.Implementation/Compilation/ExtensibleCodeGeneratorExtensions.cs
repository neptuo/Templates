using Neptuo.Web.Framework.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    public static class ExtensibleCodeGeneratorExtensions
    {
        public static void AddExtension<TCodeObject, TCodeGenerator>(this IExtensibleCodeGenerator codeGenerator)
            where TCodeObject : ICodeObject
        {
            codeGenerator.AddExtension(typeof(TCodeObject), typeof(TCodeGenerator));
        }
    }
}
