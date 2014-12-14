﻿using Neptuo.Templates.Compilation.CodeObjects;
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

        public void Visit(IPropertyDescriptor propertyDescriptor, IVisitorContext context)
        {
            Guard.NotNull(propertyDescriptor, "propertyDescriptor");
            Guard.NotNull(context, "context");
            Context = context;
            Visit(propertyDescriptor);
        }

        public void Visit(ICodeObject codeObject, IVisitorContext context)
        {
            Guard.NotNull(codeObject, "codeObject");
            Guard.NotNull(context, "context");
            Context = context;
            Visit(codeObject);
        }

        protected void Visit(IEnumerable<IPropertyDescriptor> properties)
        {
            foreach (IPropertyDescriptor propertyDescriptor in properties)
                Visit(propertyDescriptor);
        }

        protected void Visit(IPropertyDescriptor propertyDescriptor)
        {
            ListAddPropertyDescriptor listAdd = propertyDescriptor as ListAddPropertyDescriptor;
            if (listAdd != null)
            {
                Visit(listAdd);
                return;
            }

            SetPropertyDescriptor set = propertyDescriptor as SetPropertyDescriptor;
            if (set != null)
            {
                Visit(set);
                return;
            }

            MethodInvokePropertyDescriptor methodInvoke = propertyDescriptor as MethodInvokePropertyDescriptor;
            if (methodInvoke != null)
            {
                Visit(methodInvoke);
                return;
            }

            DictionaryAddPropertyDescriptor dictionaryAdd = propertyDescriptor as DictionaryAddPropertyDescriptor;
            if (dictionaryAdd != null)
            {
                Visit(dictionaryAdd);
                return;
            }

            VisitUnknown(propertyDescriptor);
        }

        protected virtual void VisitUnknown(IPropertyDescriptor propertyDescriptor)
        { }

        /// <summary>
        /// Visits code object.
        /// </summary>
        /// <param name="codeObject">Code object to visit.</param>
        protected abstract void Visit(ICodeObject codeObject);

        protected virtual void Visit(ListAddPropertyDescriptor propertyDescriptor)
        {
            foreach (ICodeObject codeObject in propertyDescriptor.Values)
                Visit(codeObject);
        }

        protected virtual void Visit(SetPropertyDescriptor propertyDescriptor)
        {
            Visit(propertyDescriptor.Value);
        }

        protected virtual void Visit(MethodInvokePropertyDescriptor propertyDescriptor)
        {
            foreach (ICodeObject codeObject in propertyDescriptor.Parameters)
                Visit(codeObject);
        }

        protected virtual void Visit(DictionaryAddPropertyDescriptor propertyDescriptor)
        {
            foreach (KeyValuePair<ICodeObject, ICodeObject> item in propertyDescriptor.Values)
            {
                Visit(item.Key);
                Visit(item.Value);
            }
        }
    }
}