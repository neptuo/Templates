// Guids.cs
// MUST match guids.h
using System;

namespace Test.Templates.VisualStudio.UI
{
    static class GuidList
    {
        public const string PkgString = "227871fe-372f-4a06-b585-487216c06208";
        public const string CmdSetString = "ec907c52-b3b6-4e47-b79e-598bae13f7a8";

        public static readonly Guid CmdSet = new Guid(CmdSetString);
    };
}