using Neptuo.Web.Framework.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    public interface IParserServiceContext
    {
        IServiceProvider ServiceProvider { get; }
        IPropertyDescriptor PropertyDescriptor { get; }
    }
}
