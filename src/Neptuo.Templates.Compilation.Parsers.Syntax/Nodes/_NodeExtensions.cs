using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes
{
    /// <summary>
    /// Common extensions for <see cref="INode"/>.
    /// </summary>
    public static class _NodeExtensions
    {
        public static T FindFirstAncestorOfType<T>(this INode node)
            where T: class
        {
            INode parent = node.Parent;
            while (parent != null)
            {
                T result = parent as T;
                if (result != null)
                    return result;

                parent = parent.Parent;
            }

            return null;
        }

        public static T FindSelfOrFirstAncestorOfType<T>(this INode node)
            where T: class
        {
            INode parent = node;
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
