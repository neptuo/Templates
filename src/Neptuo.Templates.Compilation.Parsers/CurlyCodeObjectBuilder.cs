﻿using Neptuo.Models.Features;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.CodeObjects.Features;
using Neptuo.Templates.Compilation.Parsers.Descriptors;
using Neptuo.Templates.Compilation.Parsers.Descriptors.Features;
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
            ComponentCodeObject codeObject = new ComponentCodeObject();

            bool result = true;
            FieldCollectionCodeObject fields = new FieldCollectionCodeObject();
            if (TryBuildAttributes(node.Attributes, fields, context))
                codeObject.Add<IFieldCollectionCodeObject>(fields);
            else
                result = false;

            // If all binding was ok, return code object wrapped in list.
            if (result)
                return new CodeObjectCollection().Add(codeObject);

            // Otherwise return null.
            return null;
        }

        protected bool TryBuildAttributes(IEnumerable<CurlyAttributeSyntax> attributeNodes, IFieldCollectionCodeObject codeObject, ICodeObjectBuilderContext context)
        {
            IFieldEnumerator feature;
            if (!descriptor.TryWithFields(out feature))
                return false;

            INameNormalizer nameNormalizer = context.ParserProvider.WithPropertyNormalizer();
            Dictionary<string, IFieldDescriptor> fields = new Dictionary<string, IFieldDescriptor>();
            foreach (IFieldDescriptor fieldDescriptor in feature)
                fields[nameNormalizer.PrepareName(fieldDescriptor.Name)] = fieldDescriptor;

            bool result = true;
            foreach (CurlyAttributeSyntax node in attributeNodes)
            {
                string nodeName = nameNormalizer.PrepareName(node.NameToken.Text);
                IFieldDescriptor fieldDescriptor;
                if (fields.TryGetValue(nodeName, out fieldDescriptor))
                {
                    IEnumerable<ICodeProperty> codeProperties = context.ParserProvider.WithPropertyBuilder().TryBuild(node.Value, context.CreatePropertyContext(fieldDescriptor));
                    if (codeProperties == null)
                    {
                        result = false;
                        continue;
                    }

                    foreach (ICodeProperty codeProperty in codeProperties)
                        codeObject.AddProperty(codeProperty);
                }
            }

            return result;
        }
    }
}
