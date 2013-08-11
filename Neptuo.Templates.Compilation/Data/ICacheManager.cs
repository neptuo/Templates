using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Data
{
    public interface ICacheManager
    {
        ICacheItem Item(params object[] keys);
        void Clear();
    }
}
