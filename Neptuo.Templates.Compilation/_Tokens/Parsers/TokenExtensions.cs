using Neptuo.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Common extensions on <see cref="Token"/>.
    /// </summary>
    public static class TokenExtensions
    {
        /// <summary>
        /// Returns <c>true</c> if attribute <paramref name="attributeName"/> is defined on <paramref name="token"/>.
        /// </summary>
        /// <param name="token">Source token.</param>
        /// <param name="attributeName">Attribute name.</param>
        /// <returns><c>true</c> if <paramref name="attributeName"/> is defined on <paramref name="token"/>; <c>false</c> otherwise.</returns>
        public static bool HasAttribute(this Token token, string attributeName)
        {
            Guard.NotNull(token, "element");
            Guard.NotNullOrEmpty(attributeName, "attributeName");
            return token.Attributes.Any(a => a.Name == attributeName);
        }

        /// <summary>
        /// Returns value of attribute <paramref name="attributeName"/> defined on <paramref name="token"/>.
        /// </summary>
        /// <param name="token">Source token.</param>
        /// <param name="attributeName">Attribute name.</param>
        /// <returns>Value of attribute; <c>null</c> if attribute is not found.</returns>
        public static string GetAttributeValue(this Token token, string attributeName)
        {
            Guard.NotNull(token, "element");
            Guard.NotNullOrEmpty(attributeName, "attributeName");
            TokenAttribute attribute = token.Attributes.FirstOrDefault(a => a.Name == attributeName);
            if (attribute == null)
                return null;

            return attribute.Value;
        }
    }
}
