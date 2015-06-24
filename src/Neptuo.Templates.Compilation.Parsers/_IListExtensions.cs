using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public static class _IListExtensions
    {
        public static void AddRange<T>(this IList<T> list, IEnumerable<T> newItems)
        {
            foreach (T item in newItems)
                list.Add(item);
        }
    }
}
