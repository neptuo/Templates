﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.ViewActivators
{
    /// <summary>
    /// View activator, that always returns null. View is always recompiled and never cached.
    /// </summary>
    public class NullViewActivator : IViewActivator
    {
        public object Activate(string viewContent, IViewActivatorContext context)
        {
            return null;
        }
    }
}
