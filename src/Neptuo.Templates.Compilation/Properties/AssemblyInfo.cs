using Neptuo.Templates.Compilation;
using System;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: CLSCompliant(false)]
[assembly: ComVisible(false)]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyCompany("Neptuo")]
[assembly: AssemblyCopyright("Copyright ©  2015")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]


[assembly: AssemblyTitle("Neptuo.Templates.Compilation")]
[assembly: AssemblyDescription("Compilation pipeline for template processing.")]

[assembly: AssemblyVersion(VersionInfo.Version)]
[assembly: AssemblyInformationalVersion(VersionInfo.Version)]
[assembly: AssemblyFileVersion(VersionInfo.Version)]
