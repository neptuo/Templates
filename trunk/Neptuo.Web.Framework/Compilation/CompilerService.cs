using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    public class CompilerService
    {
        public IContentCompiler ContentCompiler { get; set; }

        public List<IValueCompiler> ValueCompilers { get; set; }

        public IValueCompiler DefaultValueCompiler { get; set; }

        public CompilerService()
        {
            ValueCompilers = new List<IValueCompiler>();
            DefaultValueCompiler = new DefaultValueCompiler();
        }

        public void CompileContent(string content, CompilerContext context)
        {
            if (ContentCompiler == null)
                throw new ArgumentNullException("ContentCompiler");

            if (!ContentCompiler.GenerateCode(content, new ContentCompilerContext(context, this)))
                throw new ApplicationException("This compiler can't compiler it!");
        }

        public void CompileValue(string value, CompilerContext context)
        {
            bool generated = false;
            foreach (IValueCompiler compiler in ValueCompilers)
            {
                if (compiler.GenerateCode(value, new ValueCompilerContext(context, this)))
                {
                    generated = true;
                    break;
                }
            }

            //What if there is no value compiler? Return plain value? Register default "empty" value compiler
            if (!generated)
                DefaultValueCompiler.GenerateCode(value, new ValueCompilerContext(context, this));
        }
    }
}
