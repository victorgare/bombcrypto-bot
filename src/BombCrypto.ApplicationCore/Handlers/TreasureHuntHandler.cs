using BombCrypto.ConsoleApplication.Helpers;
using BombCrypto.CrossCutting.Helpers;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace BombCrypto.ApplicationCore.Handlers
{
    public class TreasureHuntHandler : AbstractHandler
    {
        private const int MaxRetryCount = 5;
        private const int MaxWaitTimeSeconds = 2;

        public async override Task HandleAsync(AutomationElement element)
        {
            Console.WriteLine($"::TreasureHuntHandler:: Iniciando - {DateTime.Now}");
            var pathTemplate = Path.Combine(Environment.CurrentDirectory, "Resources", "treasure-hunt-icon.png");
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
                    MouseOperations.DoubleClick(centerPoint.X, centerPoint.Y);

                    matched = true;

                    Console.WriteLine($"::TreasureHuntHandler:: Encontrado - {DateTime.Now}");
                }

                if (!matched)
                {
                    retryCount++;
                    Console.WriteLine($"::TreasureHuntHandler:: Tentativa {retryCount} - {DateTime.Now}");
                    await Task.Delay(TimeSpan.FromSeconds(MaxWaitTimeSeconds));
                }
            } while (retryCount <= MaxRetryCount && !matched);

            Console.WriteLine($"::TreasureHuntHandler:: Chamando proximo handler - {DateTime.Now}");
            await base.HandleAsync(element);
        }
    }
}
