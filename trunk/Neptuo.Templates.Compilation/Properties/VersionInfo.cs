using System;

namespace Neptuo.Templates.Compilation
{
    public static class VersionInfo
    {
        internal const string Version = "0.9.4";

        public static Version GetVersion()
        {
            return new Version(Version);
        }
    }
}
