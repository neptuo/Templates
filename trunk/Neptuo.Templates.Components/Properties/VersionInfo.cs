using System;

namespace Neptuo.Templates.Components
{
    public static class VersionInfo
    {
        internal const string Version = "2.13.0";

        public static Version GetVersion()
        {
            return new Version(Version);
        }
    }
}
