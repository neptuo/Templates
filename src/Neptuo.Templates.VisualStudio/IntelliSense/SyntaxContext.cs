using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio.IntelliSense
{
    /// <summary>
    /// Token list represented as <see cref="ISyntaxNode"/>.
    /// </summary>
    public class SyntaxContext
    {
        private readonly TokenContext tokenContext;
        private readonly ISyntaxNodeFactory nodeFactory;

        public ISyntaxNode RootNode { get; private set; }
        public event Action<SyntaxContext> OnRootNodeChanged;

        public SyntaxContext(TokenContext tokenContext, ISyntaxNodeFactory nodeFactory)
        {
            Ensure.NotNull(tokenContext, "tokenContext");
            Ensure.NotNull(nodeFactory, "nodeFactory");
            this.tokenContext = tokenContext;
            this.nodeFactory = nodeFactory;

            tokenContext.OnTokensChanged += OnTokensChanged;
            BuildRootNode();
        }

        private void OnTokensChanged(TokenContext e)
        {
            BuildRootNode();
            if (OnRootNodeChanged != null)
                OnRootNodeChanged(this);
        }

        protected void BuildRootNode()
        {
            RootNode = nodeFactory.Create(tokenContext.Tokens.ToList());
        }
    }
}
