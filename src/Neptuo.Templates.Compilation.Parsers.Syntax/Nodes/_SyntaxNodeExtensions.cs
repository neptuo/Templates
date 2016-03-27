using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes
{
    /// <summary>
    /// Common extensions for <see cref="ISyntaxNode"/>.
    /// </summary>
    public static class _SyntaxNodeExtensions
    {
        public static T FindFirstAncestorOfType<T>(this ISyntaxNode node)
            where T: class
        {
            ISyntaxNode parent = node.Parent;
            while (parent != null)
            {
                T result = parent as T;
                if (result != null)
                    return result;

                parent = parent.Parent;
            }

            return null;
        }

        public static T FindSelfOrFirstAncestorOfType<T>(this ISyntaxNode node)
            where T: class
        {
            ISyntaxNode parent = node;
            while (parent != null)
            {
                T result = parent as T;
                if (result != null)
                    return result;

                parent = parent.Parent;
            }

            return null;
        }
    }
}
