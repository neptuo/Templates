using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio.IntelliSense
{
    /// <summary>
    /// Token list represented as <see cref="INode"/>.
    /// </summary>
    public class SyntaxContext
    {
        private readonly TokenContext tokenContext;
        private readonly INodeFactory nodeFactory;

        public INode RootNode { get; private set; }
        public event Action<SyntaxContext> RootNodeChanged;
        public event Action<SyntaxContext, NodeException> Error;

        public SyntaxContext(TokenContext tokenContext, INodeFactory nodeFactory)
        {
            Ensure.NotNull(tokenContext, "tokenContext");
            Ensure.NotNull(nodeFactory, "nodeFactory");
            this.tokenContext = tokenContext;
            this.nodeFactory = nodeFactory;

            tokenContext.TokensChanged += OnTokensChanged;
            BuildRootNode();
        }

        private void OnTokensChanged(TokenContext e)
        {
            BuildRootNode();
            if (RootNodeChanged != null)
                RootNodeChanged(this);
        }

        protected void BuildRootNode()
        {
            try
            {
                RootNode = nodeFactory.Create(tokenContext.Tokens.ToList());
            }
            catch (NodeException e)
            {
                RootNode = null;

                if (Error != null)
                    Error(this, e);
            }
        }
    }
}
