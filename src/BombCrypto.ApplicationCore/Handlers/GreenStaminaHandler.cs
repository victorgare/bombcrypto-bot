using BombCrypto.ApplicationCore.Domain;
using BombCrypto.ConsoleApplication.Helpers;
using BombCrypto.CrossCutting.Helpers;
using MoreLinq;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace BombCrypto.ApplicationCore.Handlers
{
    public class GreenStaminaHandler : AbstractHandler
    {
        private const int MaxRetryCount = 20;
        private const int MaxWaitTimeSeconds = 2;

        public async override Task HandleAsync(Config config)
        {
            Console.WriteLine($"::GreenStaminaHandler:: Iniciando - {DateTime.Now}");

            var greenStaminaTemplate = GetTemplate("green-bar.png");
            var workButtonTemplate = GetTemplate("go-work.png");

            var retryCount = 0;
            do
            {
                config.CancellationTokenSource.Token.ThrowIfCancellationRequested();

                await ScrollDown(config.Element);

                var source = ScreenCapture.CaptureWindow(config.Element);

                var greenStaminaRectangles = ImageHelper.SearchBitmaps(greenStaminaTemplate, source);
                var workButtonRectangles = ImageHelper.SearchBitmaps(workButtonTemplate, source);

                if (greenStaminaRectangles.Any() && workButtonRectangles.Any())
                {
                    foreach (var greenStaminaRectangle in greenStaminaRectangles)
                    {
                        var bottonValue = greenStaminaRectangle.Bottom;
                        var nearestWorkButtonRectangle = workButtonRectangles.MinBy(x => System.Math.Abs((long)x.Bottom - bottonValue)).First();
                        var diff = bottonValue - nearestWorkButtonRectangle.Bottom;
                        if (diff <= 10 && diff >= -10)
                        {
                            var centerPoint = nearestWorkButtonRectangle.Center();
                            MouseOperations.MouseClick(centerPoint.X, centerPoint.Y);

                            Console.WriteLine($"::GreenStaminaHandler:: Encontrado - {DateTime.Now}");
                            retryCount--;
                        }
                    }
                }

                retryCount++;

                Console.WriteLine($"::GreenStaminaHandler:: Tentativa {retryCount} - {DateTime.Now}");
                await Task.Delay(TimeSpan.FromSeconds(MaxWaitTimeSeconds), config.CancellationTokenSource.Token);
            } while (retryCount <= MaxRetryCount);

            Console.WriteLine($"::GreenStaminaHandler:: Chamando proximo handler - {DateTime.Now}");
            await base.HandleAsync(config);
        }

        private async Task ScrollDown(AutomationElement element)
        {
            var bottonFrameTemplate = GetTemplate("heroes-list-botton-frame.png");

            var source = ScreenCapture.CaptureWindow(element);

            var bottonFrameRectangle = ImageHelper.SearchBitmap(bottonFrameTemplate, source);

            if (bottonFrameRectangle != Rectangle.Empty)
            {

                var startX = bottonFrameRectangle.Center().X;
                var startY = bottonFrameRectangle.Top - 100;

                MouseOperations.Scroll(startX, startY, -1200);

                await Task.Delay(TimeSpan.FromSeconds(MaxWaitTimeSeconds));
            }
        }
    }
}
