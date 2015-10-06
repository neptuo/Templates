using Neptuo.Exceptions.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes
{
    public static class _EnsureConditionExtensions
    {
        public static NotSupportedException NotCloneable(this EnsureConditionHelper condition, ISyntaxNode node)
        {
            Ensure.NotNull(node, "node");
            return Ensure.Exception.NotSupported("Unnable to clone because '{0}' is not cloneable.", node.GetType().FullName);
        }
    }
}
