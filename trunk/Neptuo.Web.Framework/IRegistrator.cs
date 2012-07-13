using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework
{
    /// <summary>
    /// Represents component that can register controls/extensions.
    /// </summary>
    public interface IRegistrator
    {
        Type GetControl(string tagNamespace, string tagName);

        /// <summary>
        /// Registers all controls, markup extensions and attribute handlers from passed <paramref name="newNamespace"/>.
        /// </summary>
        /// <param name="prefix">Prefix to register controls/extensions under.</param>
        /// <param name="newNamespace">Namespace to register.</param>
        void RegisterNamespace(string prefix, string newNamespace);
    }
}
