using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Neptuo.Templates
{
    public interface IGeneratedView : IDisposable
    {
        ICollection<object> Content { get; }
    }
}
