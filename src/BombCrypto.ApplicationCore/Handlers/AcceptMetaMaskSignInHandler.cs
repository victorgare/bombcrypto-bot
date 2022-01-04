using BombCrypto.ConsoleApplication.Helpers;
using BombCrypto.CrossCutting.Helpers;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace BombCrypto.ApplicationCore.Handlers
{
    public class AcceptMetaMaskSignInHandler : AbstractHandler
    {
        private const int MaxRetryCount = 10;
        private const int MaxWaitTimeSeconds = 5;

        public async override Task HandleAsync(AutomationElement element)
        {
            var pathTemplate = Path.Combine(Environment.CurrentDirectory, "Resources", "select-wallet-2.png");
            var template = (Bitmap)Image.FromFile(pathTemplate);

            var retryCount = 0;
            var matched = false;
            do
            {
                var metaMaskScreens = BrowserDetector.GetBrowses().GetByWindowTitle("MetaMask").ToList();

                foreach (var metaMaskScreen in metaMaskScreens)
                {
                    if (metaMaskScreen != null && !matched)
                    {
                        var windoRect = ScreenCapture.GetWindowBound(metaMaskScreen.MainWindowHandle);
                        ScreenHelper.MoveWindow(metaMaskScreen.MainWindowHandle, 0, 0, windoRect.Width, windoRect.Height, false);
                        var source = ScreenCapture.CaptureWindow(metaMaskScreen.MainWindowHandle);
                        var rectangle = ImageHelper.SearchBitmap(template, source);

                        if (rectangle != Rectangle.Empty)
                        {
                            var centerPoint = rectangle.Center();
                            MouseOperations.MouseClick(centerPoint.X, centerPoint.Y);

                            matched = true;
                        }
                    }
                }

                if (!matched)
                {
                    retryCount++;
                    await Task.Delay(TimeSpan.FromSeconds(MaxWaitTimeSeconds));
                }
            } while (retryCount <= MaxRetryCount && !matched);

            await base.HandleAsync(element);
        }
    }
}
