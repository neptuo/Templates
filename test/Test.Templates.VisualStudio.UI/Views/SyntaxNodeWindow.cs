using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Test.Templates.VisualStudio.UI.Views
{
    [Guid("B1B040F1-904C-4AAE-919C-616DB4BFA087")]
    public class SyntaxNodeWindow : ToolWindowPane
    {
        public SyntaxNodeWindow()
        {
            Caption = "Syntax nodes";
            BitmapResourceID = 301;
            BitmapIndex = 1;

            Content = "Hello, World!";
        }
    }
}
