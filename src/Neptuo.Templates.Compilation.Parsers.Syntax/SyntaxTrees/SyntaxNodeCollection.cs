using Neptuo.Templates.Compilation.Parsers.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.SyntaxTrees
{
    public class SyntaxNodeCollection : SyntaxNodeBase<SyntaxNodeCollection>
    {
        public IList<ISyntaxNode> Nodes { get; private set; }

        public SyntaxNodeCollection()
        {
            Nodes = new List<ISyntaxNode>();
        }

        protected override SyntaxNodeCollection CloneInternal()
        {
            SyntaxNodeCollection result = new SyntaxNodeCollection();

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

        protected override IEnumerable<Token> GetTokensInternal()
        {
            return Nodes.SelectMany(n => n.GetTokens());
        }
    }
}
