using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Data
{
    public interface ICacheItem
    {
        bool HasValue();
        void Remove();
        object GetValue();
        void SetValue();
    }
}
