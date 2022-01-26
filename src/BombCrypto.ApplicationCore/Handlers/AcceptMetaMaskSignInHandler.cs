using BombCrypto.ConsoleApplication.Helpers;
using BombCrypto.CrossCutting.Helpers;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace BombCrypto.ApplicationCore.Handlers
{
    public class AcceptMetaMaskSignInHandler : AbstractHandler
    {
        private const int MaxRetryCount = 5;
        private const int MaxWaitTimeSeconds = 5;

        public async override Task HandleAsync(AutomationElement element)
        {
            Console.WriteLine($"::AcceptMetaMaskSignInHandler:: Iniciando - {DateTime.Now}");

            var template = GetTemplate("select-wallet-2.png");

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
                            Console.WriteLine($"::AcceptMetaMaskSignInHandler:: Encontrado - {DateTime.Now}");
                        }
                    }
                }

                if (!matched)
                {
                    retryCount++;
                    Console.WriteLine($"::AcceptMetaMaskSignInHandler:: Tentativa {retryCount} - {DateTime.Now}");
                    await Task.Delay(TimeSpan.FromSeconds(MaxWaitTimeSeconds));
                }
            } while (retryCount <= MaxRetryCount && !matched);

            Console.WriteLine($"::AcceptMetaMaskSignInHandler:: Chamando proximo handler - {DateTime.Now}");
            await base.HandleAsync(element);
        }
    }
}
