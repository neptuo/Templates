using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Test.Templates.VisualStudio.UI
{
    [Guid("79D6D424-9EFA-4C1C-B0F0-D6F3E65DCE4A")]
    public class SyntaxTokenView : ToolWindowPane
    {
        public SyntaxTokenView()
        {
            Caption = "Syntax tokens";

            BitmapResourceID = 301;
            BitmapIndex = 1;
        }
    }
}
