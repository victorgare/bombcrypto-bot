using BombCrypto.ConsoleApplication.Helpers;
using BombCrypto.CrossCutting.Helpers;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace BombCrypto.ApplicationCore.Handlers
{
    public class ConnectWallerHandler : AbstractHandler
    {
        private const int MaxRetryCount = 10;
        private const int MaxWaitTimeSeconds = 2;
        public async override Task HandleAsync(AutomationElement element)
        {
            Console.WriteLine($"::ConnectWallerHandler:: Iniciando - {DateTime.Now}");

            var template = GetTemplate("connect-wallet.png");

            var retryCount = 0;
            var matched = false;
            do
            {
                var source = ScreenCapture.CaptureWindow(element);
                var rectangle = ImageHelper.SearchBitmap(template, source);

                if (rectangle != Rectangle.Empty)
                {
                    var centerPoint = rectangle.Center();
                    MouseOperations.MouseClick(centerPoint.X, centerPoint.Y);

                    matched = true;
                    Console.WriteLine($"::ConnectWallerHandler:: Encontrado - {DateTime.Now}");
                }

                if (!matched)
                {
                    retryCount++;
                    Console.WriteLine($"::ConnectWallerHandler:: Tentativa {retryCount} - {DateTime.Now}");
                    await Task.Delay(TimeSpan.FromSeconds(MaxWaitTimeSeconds));
                }

            } while (retryCount <= MaxRetryCount && !matched);

            Console.WriteLine($"::ConnectWallerHandler:: Chamando proximo handler - {DateTime.Now}");
            await base.HandleAsync(element);
        }
    }
}
