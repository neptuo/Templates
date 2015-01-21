﻿using System;

namespace Neptuo.Templates.Compilation
{
    /// <summary>
    /// Describes version of this library.
    /// </summary>
    public static class VersionInfo
    {
        /// <summary>
        /// Internal string version (used in assembly info).
        /// </summary>
        internal const string Version = "1.0.0";

        /// <summary>
        /// Version of this library.
        /// </summary>
        /// <returns></returns>
        public static Version GetVersion()
        {
            return new Version(Version);
        }
    }
}