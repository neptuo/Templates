using System;

namespace Neptuo.Templates.Components
{
    public static class VersionInfo
    {
        internal const string Version = "2.5.4";

        public static Version GetVersion()
        {
            return new Version(Version);
        }
    }
}
