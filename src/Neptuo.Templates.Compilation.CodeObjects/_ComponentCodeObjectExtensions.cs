using Neptuo.Models.Features;
using Neptuo.Templates.Compilation.CodeObjects.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    /// <summary>
    /// Common extensions for <see cref="ComponentCodeObject"/>.
    /// </summary>
    public static class _ComponentCodeObjectExtensions
    {
        /// <summary>
        /// Adds <paramref name="name"/> to the <paramref name="codeObject"/>.
        /// </summary>
        /// <param name="codeObject">Code object to add name to.</param>
        /// <param name="name">Name to add.</param>
        /// <returns><paramref name="codeObject"/>.</returns>
        public static ComponentCodeObject AddName(this ComponentCodeObject codeObject, INameCodeObject name)
        {
            Ensure.NotNull(codeObject, "codeObject");
            Ensure.NotNull(name, "name");
            codeObject.Add<INameCodeObject>(name);
            return codeObject;
        }

        /// <summary>
        /// Adds <paramref name="name"/> as <see cref="INameCodeObject"/> to the <paramref name="codeObject"/>.
        /// </summary>
        /// <param name="codeObject">Code object to add name to.</param>
        /// <param name="name">Name to add.</param>
        /// <returns><paramref name="codeObject"/>.</returns>
        public static ComponentCodeObject AddName(this ComponentCodeObject codeObject, string name)
        {
            Ensure.NotNull(codeObject, "codeObject");
            Ensure.NotNull(name, "name");
            codeObject.Add<INameCodeObject>(new NameCodeObject(name));
            return codeObject;
        }
    }
}
