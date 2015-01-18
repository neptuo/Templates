﻿using Neptuo.Collections.Specialized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Default implementation of <see cref="ICodeDomObjectContext"/>.
    /// </summary>
    public class DefaultCodeDomObjectContext : DefaultCodeDomContext, ICodeDomObjectContext
    {
        private readonly IKeyValueCollection customValues;

        public IReadOnlyKeyValueCollection CustomValues { get { return customValues; } }

        public DefaultCodeDomObjectContext(ICodeGeneratorContext generatorContext, ICodeDomConfiguration configuration, ICodeDomStructure structure, ICodeDomRegistry registry)
            : base(generatorContext, configuration, structure, registry)
        { }

        public DefaultCodeDomObjectContext(ICodeDomContext context)
            : base(context.GeneratorContext, context.Configuration, context.Structure, context.Registry)
        {
            customValues = new KeyValueCollection();
        }

        /// <summary>
        /// Sets <paramref name="value"/> with <paramref name="key"/> to <see cref="ICodeDomObjectContext.CustomValues"/>.
        /// </summary>
        /// <param name="key">Key to set.</param>
        /// <param name="value">Value to associate with <paramref name="key"/>.</param>
        public DefaultCodeDomObjectContext AddCustomValue(string key, object value)
        {
            customValues.Set(key, value);
            return this;
        }
    }
}
