﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    public interface IViewServiceContext
    {
        IDependencyProvider DependencyProvider { get; }
    }
}