using BombCrypto.ConsoleApplication.Helpers;
using BombCrypto.CrossCutting.Helpers;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace BombCrypto.ApplicationCore.Handlers
{
    public class HeroesScreenHandler : AbstractHandler
    {
        private const int MaxRetryCount = 5;
        private const int MaxWaitTimeSeconds = 2;

        public async override Task HandleAsync(AutomationElement element)
        {
            Console.WriteLine($"::HeroesScreenHandler:: Iniciando - {DateTime.Now}");

            var template = GetTemplate("hero-icon.png");

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
                    Console.WriteLine($"::HeroesScreenHandler:: Encontrado - {DateTime.Now}");
                }

                if (!matched)
                {
                    retryCount++;
                    Console.WriteLine($"::HeroesScreenHandler:: Tentativa {retryCount} - {DateTime.Now}");
                    await Task.Delay(TimeSpan.FromSeconds(MaxWaitTimeSeconds));
                }

            } while (retryCount <= MaxRetryCount && !matched);

            Console.WriteLine($"::HeroesScreenHandler:: Chamando proximo handler - {DateTime.Now}");
            await base.HandleAsync(element);
        }
    }
}
