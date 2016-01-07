// Guids.cs
// MUST match guids.h
using System;

namespace Test.Templates.VisualStudio.UI
{
    static class MyConstants
    {
        public const string PackageString = "227871fe-372f-4a06-b585-487216c06208";
        public const string CommandSetString = "ec907c52-b3b6-4e47-b79e-598bae13f7a8";

        public static readonly Guid CommandSetGuid = new Guid(CommandSetString);

        public static class CommandSet
        {
            public const int SyntaxTokenView = 0x100;
            public const int SyntaxNodeView = 0x101;
        }

    }
}