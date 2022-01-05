using BombCrypto.ConsoleApplication.Helpers;
using BombCrypto.CrossCutting.Helpers;
using System;
using System.Drawing;
using System.IO;
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
            var pathTemplate = Path.Combine(Environment.CurrentDirectory, "Resources", "connect-wallet.png");
            var template = (Bitmap)Image.FromFile(pathTemplate);

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
