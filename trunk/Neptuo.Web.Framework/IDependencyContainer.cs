using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework
{
    public interface IDependencyContainer : IDependencyProvider
    {
        IDependencyContainer RegisterInstance(Type t, string name, object instance);
        IDependencyContainer RegisterType(Type from, Type to, string name);
    }
}
