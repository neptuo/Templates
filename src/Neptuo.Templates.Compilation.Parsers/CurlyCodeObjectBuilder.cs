using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers.Descriptors;
using Neptuo.Templates.Compilation.Parsers.Normalization;
using Neptuo.Templates.Compilation.Parsers.SyntaxTrees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class CurlyCodeObjectBuilder : CodeObjectBuilderBase<CurlySyntax>
    {
        private readonly IComponentDescriptor descriptor;

        public CurlyCodeObjectBuilder(IComponentDescriptor descriptor)
        {
            Ensure.NotNull(descriptor, "descriptor");
            this.descriptor = descriptor;
        }

        protected override IEnumerable<ICodeObject> TryBuild(CurlySyntax node, ICodeObjectBuilderContext context)
        {
            //ComponentCodeObject codeObject = new ComponentCodeObject();
            throw new NotImplementedException();
        }
    }
}
