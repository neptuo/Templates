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

        protected bool TryBuildAttributes(IEnumerable<CurlyAttributeSyntax> attributeNodes, IPropertiesCodeObject codeObject, ICodeObjectBuilderContext context)
        {
            IFieldCollectionFeature feature;
            if (!descriptor.TryWithFields(out feature))
                return false;

            INameNormalizer nameNormalizer = context.Registry.WithPropertyNormalizer();
            Dictionary<string, IFieldDescriptor> fields = new Dictionary<string, IFieldDescriptor>();
            foreach (IFieldDescriptor fieldDescriptor in feature.Fields)
                fields[nameNormalizer.PrepareName(fieldDescriptor.Name)] = fieldDescriptor;

            foreach (CurlyAttributeSyntax node in attributeNodes)
            {
                string nodeName = nameNormalizer.PrepareName(node.NameToken.Text);
                IFieldDescriptor fieldDescriptor;
                if(fields.TryGetValue(nodeName, out fieldDescriptor))
                {
                    // Bind field.
                }
            }

            throw new NotImplementedException();
        }
    }
}
