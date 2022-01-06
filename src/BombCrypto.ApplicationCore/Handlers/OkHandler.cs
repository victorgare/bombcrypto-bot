using BombCrypto.ConsoleApplication.Helpers;
using BombCrypto.CrossCutting.Helpers;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace BombCrypto.ApplicationCore.Handlers
{
    public class OkHandler : AbstractHandler
    {
        private const int MaxRetryCount = 2;
        private const int MaxWaitTimeSeconds = 2;

        public async override Task HandleAsync(AutomationElement element)
        {
            Console.WriteLine($"::OkHandler:: Iniciando - {DateTime.Now}");
            var pathTemplate = Path.Combine(Environment.CurrentDirectory, "Resources", "ok.png");
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
                    Console.WriteLine($"::OkHandler:: Encontrado - {DateTime.Now}");
                }

                if (!matched)
                {
                    retryCount++;
                    Console.WriteLine($"::OkHandler:: Tentativa {retryCount} - {DateTime.Now}");
                    await Task.Delay(TimeSpan.FromSeconds(MaxWaitTimeSeconds));
                }

            } while (retryCount <= MaxRetryCount && !matched);

            Console.WriteLine($"::OkHandler:: Chamando proximo handler - {DateTime.Now}");
            await base.HandleAsync(element);

        }
    }
}
