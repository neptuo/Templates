﻿using Neptuo.Web.Framework.Compilation;
using Neptuo.Web.Framework.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Composition
{
    public class TemplateExtension : IMarkupExtension
    {
        private IViewService viewService;
        private IComponentManager componentManager;

        public string File { get; set; }

        public TemplateExtension(IViewService viewService, IComponentManager componentManager)
        {
            this.viewService = viewService;
            this.componentManager = componentManager;
        }

        public object ProvideValue(IMarkupExtensionContext context)
        {
            if (File != null)
            {
                ITemplate template = new FileTemplate(File, viewService, context.DependencyProvider);
                componentManager.AddComponent(template, null);
                return template;
            }
            return null;
        }
    }
}