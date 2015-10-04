﻿using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class RootContentBuilder : ComponentDescriptorBuilder
    {
        private readonly PropertyInfo defaultProperty;

        public RootContentBuilder(PropertyInfo defaultProperty)
        {
            Ensure.NotNull(defaultProperty, "defaultProperty");
            this.defaultProperty = defaultProperty;
        }

        protected override IComponentCodeObject CreateCodeObject(IContentBuilderContext context, IXmlElement element)
        {
            return new RootCodeObject(defaultProperty.DeclaringType);
        }

        protected override IComponentDescriptor GetComponentDescriptor(IContentBuilderContext context, ICodeObject codeObject, IXmlElement element)
        {
            return new RootComponentDescriptor(defaultProperty);
        }

        protected override bool TryBindProperty(IContentBuilderContext context, string prefix, string name, IEnumerable<IXmlNode> value)
        {
            return base.TryBindProperty(context, prefix, name, value);
        }

        protected override bool ProcessUnboundAttribute(IContentBuilderContext context, IXmlAttribute unboundAttribute)
        {
            return true;
        }
    }
}
