using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class FuncContentBuilderFactory : IContentBuilderFactory
    {
        private readonly Func<string, string, IContentBuilder> factory;

        public FuncContentBuilderFactory(Func<string, string, IContentBuilder> factory)
        {
            Guard.NotNull(factory, "factory");
            this.factory = factory;
        }

        public IContentBuilder CreateBuilder(string prefix, string tagName)
        {
            return factory(prefix, tagName);
        }
    }
}
