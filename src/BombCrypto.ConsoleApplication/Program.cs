using BombCrypto.ApplicationCore.Services;
using BombCrypto.ConsoleApplication.Helpers;
using BombCrypto.CrossCutting.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace BombCrypto.ConsoleApplication
{
    public class Program
    {
        async static Task Main(string[] args)
        {
            do
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

                    // loop through all the tabs and get the names which is the page title 
                    Condition condTabItem = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TabItem);
                    foreach (AutomationElement tabitem in root.FindAll(TreeScope.Descendants, condTabItem))
                    {
                        if (tabitem.Current.Name.Equals("Bombcrypto"))
                        {
                            tabitem.SetFocus();

                            var handler = browser.MainWindowHandle;
                            ScreenHelper.MoveWindow(handler, 0, 0, 1366, 768, false);
                            ScreenHelper.SetWindowPos(handler, IntPtr.Zero, 10, 10, 1366, 768, ScreenHelper.SetWindowPosFlags.SWP_SHOWWINDOW);
                            ScreenHelper.MoveWindow(handler, 10, 10, 1366, 768, false);

                            await processService.Process(root);
                        }
                    }
                }

                // aguarde 5 minutos e faca novamente o processo
                await Task.Delay(TimeSpan.FromMinutes(1));
            } while (true);
        }


    }
}
