using Neptuo.Compilers.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation
{
    /// <summary>
    /// Common extensions for <see cref="ICollection{IErrorInfo}"/>.
    /// </summary>
    public static class _ErrorInfoExtensions
    {
        /// <summary>
        /// Adds all errors from <paramref name="errorsToAdd"/> to <paramref name="targetCollection"/>.
        /// </summary>
        /// <param name="targetCollection">Target collection to add errors into.</param>
        /// <param name="errorsToAdd">Errors to add to the <paramref name="targetCollection"/>.</param>
        public static void AddRange(this ICollection<IErrorInfo> targetCollection, IEnumerable<IErrorInfo> errorsToAdd)
        {
            Ensure.NotNull(targetCollection, "targetCollection");
            Ensure.NotNull(errorsToAdd, "errorsToAdd");

            foreach (IErrorInfo error in errorsToAdd)
                targetCollection.Add(error);
        }
    }
}
