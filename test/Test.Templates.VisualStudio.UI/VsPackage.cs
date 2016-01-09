using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using Microsoft.Win32;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Test.Templates.VisualStudio.UI.Views;

namespace Test.Templates.VisualStudio.UI
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the information needed to show this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [Guid(MyConstants.PackageString)]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
#if DEBUG
    [ProvideToolWindow(typeof(SyntaxNodeWindow), Style = VsDockStyle.Linked, Window = EnvDTE.Constants.vsWindowKindSolutionExplorer, DockedWidth = 450, Orientation = ToolWindowOrientation.Right)]
    [ProvideToolWindow(typeof(SyntaxTokenWindow), Style = VsDockStyle.Linked, Window = EnvDTE.Constants.vsWindowKindSolutionExplorer, DockedWidth = 300, Orientation = ToolWindowOrientation.Right)]
#else
    [ProvideToolWindow(typeof(SyntaxNodeWindow))]
    [ProvideToolWindow(typeof(SyntaxTokenWindow))]
#endif
    public sealed class VsPackage : Package
    {
        /// <summary>
        /// Initializes the package.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            OleMenuCommandService commandService = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;

            CommandID tokensCommandID = new CommandID(MyConstants.CommandSetGuid, MyConstants.CommandSet.SyntaxTokenView);
            MenuCommand tokensMenuItem = new MenuCommand(OnSyntaxTokenView, tokensCommandID);
            commandService.AddCommand(tokensMenuItem);

            CommandID nodesCommandID = new CommandID(MyConstants.CommandSetGuid, MyConstants.CommandSet.SyntaxNodeView);
            MenuCommand nodesMenuItem = new MenuCommand(OnSyntaxNodeView, nodesCommandID);
            commandService.AddCommand(nodesMenuItem);


#if DEBUG
            //OnSyntaxTokenView(null, null);
            OnSyntaxNodeView(null, null);
#endif
        }

        /// <summary>
        /// When user clicks on "Open syntax tokens" menu command.
        /// </summary>
        private void OnSyntaxTokenView(object sender, EventArgs e)
        {
            SyntaxTokenWindow window = (SyntaxTokenWindow)FindToolWindow(typeof(SyntaxTokenWindow), 0, true);
            if (window != null && window.Frame != null)
            {
                IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;
                ErrorHandler.ThrowOnFailure(windowFrame.Show());
            }
        }

        /// <summary>
        /// When user clicks on "Open syntax nodes" menu command.
        /// </summary>
        private void OnSyntaxNodeView(object sender, EventArgs e)
        {
            SyntaxNodeWindow window = (SyntaxNodeWindow)FindToolWindow(typeof(SyntaxNodeWindow), 0, true);
            if (window != null && window.Frame != null)
            {
                IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;
                ErrorHandler.ThrowOnFailure(windowFrame.Show());
            }
        }
    }
}
