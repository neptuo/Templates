using Neptuo.Models.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Descriptors.Features
{
    /// <summary>
    /// Feature extensions of <see cref="IComponentDescriptor"/>.
    /// </summary>
    public static class _ComponentDescriptorExtensions
    {
        #region Fields

        public static bool TryWithFields(this IComponentDescriptor model, out IFieldEnumerator feature)
        {
            Ensure.NotNull(model, "model");
            return model.TryWith<IFieldEnumerator>(out feature);
        }

        public static IFieldEnumerator WithFields(this IComponentDescriptor model)
        {
            Ensure.NotNull(model, "model");
            return model.With<IFieldEnumerator>();
        }

        #endregion

        #region DefaultFields

        public static bool TryWithDefaultFields(this IComponentDescriptor model, out IDefaultFieldEnumerator feature)
        {
            Ensure.NotNull(model, "model");
            return model.TryWith<IDefaultFieldEnumerator>(out feature);
        }

        public static IDefaultFieldEnumerator WithDefaultFields(this IComponentDescriptor model)
        {
            Ensure.NotNull(model, "model");
            return model.With<IDefaultFieldEnumerator>();
        }

        #endregion
    }
}
