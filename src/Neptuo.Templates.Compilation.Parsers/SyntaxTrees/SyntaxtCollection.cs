using Neptuo.Templates.Compilation.Parsers.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.SyntaxTrees
{
    public class SyntaxtCollection : SyntaxNodeBase<SyntaxtCollection>
    {
        public IList<ISyntaxNode> Nodes { get; private set; }

        public SyntaxtCollection()
        {
            Nodes = new List<ISyntaxNode>();
        }

        protected override SyntaxtCollection CloneInternal()
        {
            SyntaxtCollection result = new SyntaxtCollection();

            foreach (ISyntaxNode node in Nodes)
            {
                ICloneable<ISyntaxNode> cloneableNode = node as ICloneable<ISyntaxNode>;
                if (cloneableNode != null)
                    result.Nodes.Add(cloneableNode.Clone());
                else
                    throw Ensure.Condition.NotCloneable(node);
            }

            return result;
        }

        protected override IEnumerable<ComposableToken> GetTokensInternal()
        {
            return Nodes.SelectMany(n => n.GetTokens());
        }
    }
}
