using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    public class CodeGeneratorService
    {
        public IContentCodeGenerator ContentGenerator { get; set; }
        public IList<IValueCodeGenerator> ValueGenerators { get; set; }
        public IValueCodeGenerator DefaultValueCompiler { get; set; }

        public CodeGeneratorService()
        {
            ValueGenerators = new List<IValueCodeGenerator>();
            DefaultValueCompiler = new DefaultValueGenerator();
        }

        public void ProcessContent(string content, GeneratorContext context)
        {
            if (ContentGenerator == null)
                throw new ArgumentNullException("ContentGenerator");

            if (!ContentGenerator.GenerateCode(content, new ContentGeneratorContext(context, this)))
                throw new ApplicationException("This content generator can't process it!");
        }

        public void ProcessValue(string value, GeneratorContext context)
        {
            bool generated = false;
            foreach (IValueCodeGenerator compiler in ValueGenerators)
            {
                if (compiler.GenerateCode(value, new ValueGeneratorContext(context, this)))
                {
                    generated = true;
                    break;
                }
            }

            //What if there is no value compiler? Return plain value? Register default "empty" value generator
            if (!generated)
                DefaultValueCompiler.GenerateCode(value, new ValueGeneratorContext(context, this));
        }
    }
}
