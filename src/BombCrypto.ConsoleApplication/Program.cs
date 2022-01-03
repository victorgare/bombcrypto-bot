using BombCrypto.ApplicationCore.Services;
using BombCrypto.ConsoleApplication.Helpers;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace BombCrypto.ConsoleApplication
{
    public class Program
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);


        async static Task Main(string[] args)
        {
            var processService = new ProcessService();

            var browsers = BrowserDetector.GetBrowses();
            foreach (var browser in browsers)
            {
                // the chrome process must have a window 
                if (browser.MainWindowHandle == IntPtr.Zero)
                {
                    continue;
                }

                // to find the tabs we first need to locate something reliable - the 'New Tab' button 
                AutomationElement root = AutomationElement.FromHandle(browser.MainWindowHandle);
                //Condition condNewTab = new PropertyCondition(AutomationElement.NameProperty, "Nova guia");
                ////Condition condNewTab = new PropertyCondition(AutomationElement.NameProperty, "New Tab");
                //AutomationElement elmNewTab = root.FindFirst(TreeScope.Descendants, condNewTab);

                //// get the tabstrip by getting the parent of the 'new tab' button 
                //TreeWalker treewalker = TreeWalker.ControlViewWalker;
                //AutomationElement elmTabStrip = treewalker.GetParent(elmNewTab);

                // loop through all the tabs and get the names which is the page title 
                Condition condTabItem = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TabItem);
                foreach (AutomationElement tabitem in root.FindAll(TreeScope.Descendants, condTabItem))
                {
                    if (tabitem.Current.Name.Equals("Bombcrypto"))
                    {

                        tabitem.SetFocus();

                        var handler = browser.MainWindowHandle;
                        //var handler = (IntPtr)tabitem.Current.NativeWindowHandle;
                        SetWindowPos(handler, IntPtr.Zero, 0, 0, 1280, 720, SetWindowPosFlags.SWP_SHOWWINDOW);
                        MoveWindow(handler, 0, 0, 1280, 729, false);

                        await processService.Process(root);
                    }
                }
            }
        }


    }

    public enum SpecialWindowHandles
    {
        HWND_TOP = 0,
        HWND_BOTTOM = 1,
        HWND_TOPMOST = -1,
        HWND_NOTOPMOST = -2
    }

    [Flags]
    public enum SetWindowPosFlags : uint
    {
        SWP_ASYNCWINDOWPOS = 0x4000,

        SWP_DEFERERASE = 0x2000,

        SWP_DRAWFRAME = 0x0020,

        SWP_FRAMECHANGED = 0x0020,

        SWP_HIDEWINDOW = 0x0080,

        SWP_NOACTIVATE = 0x0010,

        SWP_NOCOPYBITS = 0x0100,

        SWP_NOMOVE = 0x0002,

        SWP_NOOWNERZORDER = 0x0200,

        SWP_NOREDRAW = 0x0008,

        SWP_NOREPOSITION = 0x0200,

        SWP_NOSENDCHANGING = 0x0400,

        SWP_NOSIZE = 0x0001,

        SWP_NOZORDER = 0x0004,

        SWP_SHOWWINDOW = 0x0040,
    }
}
