using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.PreProcessing
{
    /// <summary>
    /// Base implementation of <see cref="IVisitor"/> which does through nodes of AST.
    /// </summary>
    public abstract class VisitorBase : IVisitor
    {
        /// <summary>
        /// Context inforation.
        /// </summary>
        protected IVisitorContext Context { get; private set; }

        public void Visit(ICodeProperty codeProperty, IVisitorContext context)
        {
            Guard.NotNull(codeProperty, "codeProperty");
            Guard.NotNull(context, "context");
            Context = context;
            Visit(codeProperty);
        }

        public void Visit(ICodeObject codeObject, IVisitorContext context)
        {
            Guard.NotNull(codeObject, "codeObject");
            Guard.NotNull(context, "context");
            Context = context;
            Visit(codeObject);
        }

        protected void Visit(IEnumerable<ICodeProperty> codeProperties)
        {
            foreach (ICodeProperty codeProperty in codeProperties)
                Visit(codeProperty);
        }

        protected void Visit(ICodeProperty codeProperty)
        {
            ListAddCodeProperty listAdd = codeProperty as ListAddCodeProperty;
            if (listAdd != null)
            {
                Visit(listAdd);
                return;
            }

            SetCodeProperty set = codeProperty as SetCodeProperty;
            if (set != null)
            {
                Visit(set);
                return;
            }

            MethodInvokeCodeProperty methodInvoke = codeProperty as MethodInvokeCodeProperty;
            if (methodInvoke != null)
            {
                Visit(methodInvoke);
                return;
            }

            DictionaryAddCodeProperty dictionaryAdd = codeProperty as DictionaryAddCodeProperty;
            if (dictionaryAdd != null)
            {
                Visit(dictionaryAdd);
                return;
            }

            VisitUnknown(codeProperty);
        }

        /// <summary>
        /// Any unknown code property type is going there.
        /// </summary>
        /// <param name="codeProperty">Uknown code property type.</param>
        protected virtual void VisitUnknown(ICodeProperty codeProperty)
        { }

        /// <summary>
        /// Visits code object.
        /// </summary>
        /// <param name="codeObject">Code object to visit.</param>
        protected abstract void Visit(ICodeObject codeObject);

        protected virtual void Visit(ListAddCodeProperty codeProperty)
        {
            foreach (ICodeObject codeObject in codeProperty.Values)
                Visit(codeObject);
        }

        protected virtual void Visit(SetCodeProperty codeProperty)
        {
            Visit(codeProperty.Value);
        }

        protected virtual void Visit(MethodInvokeCodeProperty codeProperty)
        {
            foreach (ICodeObject codeObject in codeProperty.Parameters)
                Visit(codeObject);
        }

        protected virtual void Visit(DictionaryAddCodeProperty codeProperty)
        {
            foreach (KeyValuePair<ICodeObject, ICodeObject> item in codeProperty.Values)
            {
                Visit(item.Key);
                Visit(item.Value);
            }
        }
    }
}
