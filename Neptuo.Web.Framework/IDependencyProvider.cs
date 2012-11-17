﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework
{
    public interface IDependencyProvider
    {
        IDependencyContainer CreateChildContainer();
        object Resolve(Type t, string name);
        IEnumerable<object> ResolveAll(Type t);
    }
}
