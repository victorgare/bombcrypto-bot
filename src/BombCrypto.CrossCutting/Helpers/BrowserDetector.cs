using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BombCrypto.ConsoleApplication.Helpers
{
    public static class BrowserDetector
    {
        private static readonly Dictionary<string, string> browsers = new Dictionary<string, string>
                                                  {
                                                      {
                                                          "firefox", "Mozilla Firefox"
                                                      },
                                                      {
                                                          "chrome", "Google Chrome"
                                                      },
                                                      {
                                                          "iexplore", "Internet Explorer"
                                                      },
                                                      {
                                                          "MicrosoftEdgeCP", "Microsoft Edge"
                                                      },
                                                      {
                                                          "msedge", "Microsoft Edge"
                                                      }
                                                      // add other browsers
                                                  };

        public static bool BrowserIsOpen()
        {
            return Process.GetProcesses().Any(IsBrowserWithWindow);
        }

        private static bool IsBrowserWithWindow(Process process)
        {
            return browsers.TryGetValue(process.ProcessName, out var browserTitle); // && process.MainWindowTitle.Contains(browserTitle);
        }

        public static List<Process> GetBrowses()
        {
            return Process.GetProcesses().Where(IsBrowserWithWindow).ToList();
        }

        public static List<Process> GetByWindowTitle(this List<Process> processes, string searchString)
        {
            var responseList = new List<Process>();
            foreach (var process in processes)
            {
                if (process.MainWindowTitle.Contains(searchString))
                {
                    responseList.Add(process);
                }
            }

            return responseList;
        }
    }
}
