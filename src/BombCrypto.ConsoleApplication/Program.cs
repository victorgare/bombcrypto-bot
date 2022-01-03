using AForge.Imaging;
using BombCrypto.ConsoleApplication.Helpers;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
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


        static void Main(string[] args)
        {
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
                Condition condNewTab = new PropertyCondition(AutomationElement.NameProperty, "Nova guia");
                //Condition condNewTab = new PropertyCondition(AutomationElement.NameProperty, "New Tab");
                AutomationElement elmNewTab = root.FindFirst(TreeScope.Descendants, condNewTab);

                // get the tabstrip by getting the parent of the 'new tab' button 
                TreeWalker treewalker = TreeWalker.ControlViewWalker;
                AutomationElement elmTabStrip = treewalker.GetParent(elmNewTab);

                // loop through all the tabs and get the names which is the page title 
                Condition condTabItem = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TabItem);
                foreach (AutomationElement tabitem in elmTabStrip.FindAll(TreeScope.Children, condTabItem))
                {
                    if (tabitem.Current.Name.Equals("Bombcrypto"))
                    {

                        tabitem.SetFocus();

                        var handler = browser.MainWindowHandle;
                        //var handler = (IntPtr)tabitem.Current.NativeWindowHandle;
                        SetWindowPos(handler, IntPtr.Zero, 0, 0, 1280, 720, SetWindowPosFlags.SWP_SHOWWINDOW);
                        MoveWindow(handler, 0, 0, 1280, 729, false);


                        var printScreen = ScreenCapture.CaptureWindow(root);
                        var pathTemplate = Path.Combine(Environment.CurrentDirectory, "Resources", "Connect_Wallet.png");
                        var template = (Bitmap)System.Drawing.Image.FromFile(pathTemplate);

                        FindImage(printScreen, template);
                        //printScreen.Save(@"C:\Users\Victor\Desktop\snippetsource.jpg", ImageFormat.Jpeg);
                    }
                }

            }
        }

        private static void FindImage(Bitmap imgToSearch, Bitmap imageToFind)
        {
            // create template matching algorithm's instance
            // (set similarity threshold to 92.1%)

            ExhaustiveTemplateMatching tm = new ExhaustiveTemplateMatching(0.921f);
            // find all matchings with specified above similarity

            var imageToSearchFormated = imgToSearch.ConvertToFormat(PixelFormat.Format24bppRgb);
            var imageToFindFormated = imageToFind.ConvertToFormat(PixelFormat.Format24bppRgb);
            TemplateMatch[] matchings = tm.ProcessImage(imageToSearchFormated, imageToFindFormated);
            // highlight found matchings

            BitmapData data = imgToSearch.LockBits(
                 new Rectangle(0, 0, imgToSearch.Width, imgToSearch.Height),
                 ImageLockMode.ReadWrite, imgToSearch.PixelFormat);
            foreach (TemplateMatch m in matchings)
            {

                Drawing.Rectangle(data, m.Rectangle, Color.White);

                Console.WriteLine(m.Rectangle.Location.ToString());

                MouseOperations.MouseClick(m.Rectangle.X, m.Rectangle.Y);
            }

            imgToSearch.UnlockBits(data);
        }
    }

    public static class Extension
    {
        public static Bitmap ConvertToFormat(this Bitmap image, PixelFormat format)
        {
            Bitmap copy = new Bitmap(image.Width, image.Height, format);
            using (Graphics gr = Graphics.FromImage(copy))
            {
                gr.DrawImage(image, new Rectangle(0, 0, copy.Width, copy.Height));
            }
            return copy;
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
