using Neptuo.Collections.Specialized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Default implementation of <see cref="ICodeDomNaming"/>.
    /// </summary>
    public class CodeDomDefaultNaming : ICodeDomNaming
    {
        private readonly IKeyValueCollection customValues;

        public IReadOnlyKeyValueCollection CustomValues { get { return customValues; } }

        public string NamespaceName { get; private set; }
        public string ClassName { get; private set; }
        public string FullClassName { get; private set; }

        public CodeDomDefaultNaming(string namespaceName, string className)
        {
            Ensure.NotNullOrEmpty(className, "className");
            NamespaceName = namespaceName;
            ClassName = className;

            if (!String.IsNullOrEmpty(NamespaceName))
                FullClassName = NamespaceName + "." + ClassName;
            else
                FullClassName = ClassName;

            customValues = new KeyValueCollection();
        }

        /// <summary>
        /// Adds custom value <paramref name="value"/> with key <paramref name="key"/>.
        /// If such a key already exist in collection, value is overriden with the new one.
        /// </summary>
        /// <param name="key">Key to set.</param>
        /// <param name="value">Value to associate with <paramref name="key"/>.</param>
        /// <returns>Self.</returns>
        public CodeDomDefaultNaming AddCustomValue(string key, object value)
        {
            customValues.Add(key, value);
            return this;
        }
    }
}
