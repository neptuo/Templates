using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.AssemblyScanning
{
    /// <summary>
    /// Common extensions for <see cref="TypeScanner"/>.
    /// </summary>
    public static class _TypeScannerExtensions
    {
        /// <summary>
        /// Adds type filter on <paramref name="scanner"/> to filter out interfaces.
        /// </summary>
        /// <param name="scanner">Type scanner to apply filter on.</param>
        /// <returns>Return from calling <see cref="TypeScanner.AddTypeFilter"/>.</returns>
        public static TypeScanner AddTypeFilterOnInterface(this TypeScanner scanner)
        {
            Guard.NotNull(scanner, "scanner");
            return scanner.AddTypeFilter(t => t.IsInterface);
        }

        /// <summary>
        /// Adds type filter on <paramref name="scanner"/> to filter out abstract classes.
        /// </summary>
        /// <param name="scanner">Type scanner to apply filter on.</param>
        /// <returns>Return from calling <see cref="TypeScanner.AddTypeFilter"/>.</returns>
        public static TypeScanner AddTypeFilterOnAbstract(this TypeScanner scanner)
        {
            Guard.NotNull(scanner, "scanner");
            return scanner.AddTypeFilter(t => t.IsAbstract);
        }
    }
}
