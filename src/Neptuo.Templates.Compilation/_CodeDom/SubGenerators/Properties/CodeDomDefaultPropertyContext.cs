using Neptuo.Collections.Specialized;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Default implementation of <see cref="ICodeDomPropertyContext"/>.
    /// </summary>
    public class CodeDomDefaultPropertyContext : CodeDomDefaultContext, ICodeDomPropertyContext
    {
        private readonly IKeyValueCollection customValues;

        public IReadOnlyKeyValueCollection CustomValues { get { return customValues; } }

        public CodeExpression PropertyTarget { get; private set; }

        public CodeDomDefaultPropertyContext(ICodeDomContext context, CodeExpression propertyTarget)
            : base(context.GeneratorContext, context.Configuration, context.Structure, context.Registry)
        {
            Guard.NotNull(propertyTarget, "propertyTarget");
            PropertyTarget = propertyTarget;
            customValues = new KeyValueCollection();
        }

        /// <summary>
        /// Sets <paramref name="value"/> with <paramref name="key"/> to <see cref="ICodeDomObjectContext.CustomValues"/>.
        /// </summary>
        /// <param name="key">Key to set.</param>
        /// <param name="value">Value to associate with <paramref name="key"/>.</param>
        public CodeDomDefaultPropertyContext AddCustomValue(string key, object value)
        {
            customValues.Set(key, value);
            return this;
        }
    }
}
