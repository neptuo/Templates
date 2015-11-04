﻿using Neptuo.Models.Features;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public static class _IContentBuilderContextExtensions
    {
        #region ComponentCodeObject

        public static IContentBuilderContext CodeObject(this IContentBuilderContext context, ICodeObject codeObject)
        {
            Ensure.NotNull(context, "context");
            context.CustomValues["CodeObject"] = codeObject;
            return context;
        }

        public static ICodeObject CodeObject(this IContentBuilderContext context)
        {
            Ensure.NotNull(context, "context");
            return (ICodeObject)context.CustomValues["CodeObject"];
        }

        public static IFieldCollectionCodeObject CodeObjectAsProperties(this IContentBuilderContext context)
        {
            Ensure.NotNull(context, "context");

            ICodeObject codeObject = CodeObject(context);
            IFieldCollectionCodeObject fields = codeObject as IFieldCollectionCodeObject;
            if (fields != null)
                return fields;

            IFeatureModel featureObject = codeObject as IFeatureModel;
            if (featureObject != null)
                return featureObject.With<IFieldCollectionCodeObject>();

            throw new NotSupportedException();
        }

        #endregion

        #region ComponentDescriptor

        public static IComponentDescriptor ComponentDescriptor(this IContentBuilderContext context)
        {
            Ensure.NotNull(context, "context");
            return (IComponentDescriptor)context.CustomValues["ComponentDescriptor"];
        }

        public static IContentBuilderContext ComponentDescriptor(this IContentBuilderContext context, IComponentDescriptor componentDescriptor)
        {
            Ensure.NotNull(context, "context");
            context.CustomValues["ComponentDescriptor"] = componentDescriptor;
            return context;
        }

        #endregion

        #region BindPropertiesContext

        public static BindContentPropertiesContext BindPropertiesContext(this IContentBuilderContext context)
        {
            Ensure.NotNull(context, "context");
            return (BindContentPropertiesContext)context.CustomValues["BindPropertiesContext"];
        }

        public static IContentBuilderContext BindPropertiesContext(this IContentBuilderContext context, BindContentPropertiesContext bindPropertiesContext)
        {
            Ensure.NotNull(context, "context");
            context.CustomValues["BindPropertiesContext"] = bindPropertiesContext;
            return context;
        }

        #endregion

        #region DefaultProperty

        public static IFieldDescriptor DefaultField(this IContentBuilderContext context)
        {
            Ensure.NotNull(context, "context");
            return (IFieldDescriptor)context.CustomValues["DefaultField"];
        }

        public static IContentBuilderContext DefaultField(this IContentBuilderContext context, IFieldDescriptor defaultField)
        {
            Ensure.NotNull(context, "defaultField");
            context.CustomValues["DefaultField"] = defaultField;
            return context;
        }

        #endregion
    }
}