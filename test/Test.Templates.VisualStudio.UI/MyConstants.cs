﻿// Guids.cs
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

        public static class ToolWindow
        {
            public const string SyntaxTokenWindowString = "79D6D424-9EFA-4C1C-B0F0-D6F3E65DCE4A";
            public const string SyntaxNodeWindowString = "B1B040F1-904C-4AAE-919C-616DB4BFA087";
        }
    }
}